using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Searcher;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [SerializeField] private float _speed;
    [SerializeField] private float _airControl;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _groundCheckDistance;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private PolygonCollider2D _hurtBox;
    [SerializeField] private List<GameplayAction> _actions;
    [SerializeField] private GameplayAction _nextAction;

    private float _lastPositionX;
    private Vector2 _direction;
    private Rigidbody2D _rigidbody2D;
    private SpriteRenderer _spriteRenderer;
    private State _currentState;
    private InputController _inputController;
    private Sprite _lastSprite;
    
    private readonly List<Vector2> _points = new();
    
    
    public Animator _animator;
    public Rigidbody2D Rigidbody2D => _rigidbody2D;
    public SpriteRenderer SpriteRenderer => _spriteRenderer;
    public float Speed => _speed;
    public float JumpForce => _jumpForce;
    public float AirControl => _airControl;
    public List<GameplayAction>Actions => _actions;
    public bool IsGrounded {
        get {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, _groundCheckDistance, _groundLayer);
            return hit.collider != null;
        }
    }
    

    public GameplayAction NextAction
    {
        get => _nextAction;
        set => _nextAction = value;
    }

    private void Awake() {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _inputController = GetComponent<InputController>();
        
        SwitchState(new MoveState(gameObject));
    }

    private void Start() {
        _nextAction = _actions.First();
        
    }

    private void Update() {
        
        Flip();
        _currentState.OnUpdate();
        
        // Transitions
        if (_currentState is MoveState && _inputController.IsJumping && IsGrounded) {
            SwitchState(new JumpState(gameObject));
            return;
        }

        if (_currentState is JumpState && Rigidbody2D.linearVelocityY > 0  )
        {
            SwitchState(new FlyState(gameObject));
            return;
        }
        
        if(_currentState is FlyState && IsGrounded)
        { 
           SwitchState(new LandState(gameObject));
        }
         
        if (_currentState is LandState) {
            SwitchState(new MoveState(gameObject));
            
        }

        if (_currentState is MoveState && _inputController.IsAttacking.Value) {
            SwitchState(new AttackState(gameObject, _nextAction.Speed));
            return;
        }

        if (_currentState is AttackState { IsFinished: true }) {
            SwitchState(new MoveState(gameObject));
            return;
        }

        if (_currentState is MoveState && _inputController.IsHeavyAttacking.Value)
        {
            SwitchState(new HeavyAttackState(gameObject, _nextAction.Speed));
            return;
        }
        
        if (_currentState is HeavyAttackState { IsFinished: true })
        {
            SwitchState(new MoveState(gameObject));
            return;
        }
    }
    
    private void SwitchState(State state) {
        Debug.Log($"Exiting State : {_currentState?.GetType().Name}");
        _currentState?.OnExit();
        _currentState = state;
        _currentState?.OnEnter();
        Debug.Log($"Entering State : {_currentState?.GetType().Name}");
    }

    private bool _hurtBoxActive;

    public void OnAttackActive() {
        _hurtBoxActive = true;
        _lastSprite = null;
        _hurtBox.enabled = true;
    }

    public void OnAttackFinished() {
        _hurtBoxActive = false;
        _hurtBox.enabled = false;
    }

    private void LateUpdate() {
        if (!_hurtBoxActive) return;
        
        Sprite sprite = _spriteRenderer.sprite;
        if (sprite == _lastSprite) return;
        _lastSprite = sprite;

        int shapeCount = sprite != null ? sprite.GetPhysicsShapeCount() : 0;
        _hurtBox.pathCount = shapeCount;

        for (int i = 0; i < shapeCount; i++) {
            sprite.GetPhysicsShape(i, _points);

            if (_spriteRenderer.flipX)
                for (int p = 0; p < _points.Count; p++)
                    _points[p] = new Vector2(-_points[p].x, _points[p].y);

            _hurtBox.SetPath(i, _points);
        }
    }
    
    private void Flip() {
        if (_inputController.Move.x < 0)
        {
            _spriteRenderer.flipX = true;
        }
        else if (_inputController.Move.x > 0)
        {
            _spriteRenderer.flipX = false;
        }
    }
}