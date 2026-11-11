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
    [SerializeField] private float _knockbackHorizontalForce;
    [SerializeField] private float _knockbackVerticalForce;
    [SerializeField] private float _comboResetTime;
    
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private PolygonCollider2D _hurtBox;
    [SerializeField] private List<GameplayAction> _attacks;
    [SerializeField] private List<GameplayAction> _lightAttacks;
    [SerializeField] private GameplayAction _nextAction;
    
    
    private float _lastPositionX;
    private int _receivedHitCount = 0;
    private float _lastHitTime = -1f;
    
    private Vector2 _direction;
    private Vector2 _knockbackDirection;
    private Rigidbody2D _rigidbody2D;
    private SpriteRenderer _spriteRenderer;
    private State _currentState;
    private InputController _inputController;
    private Sprite _lastSprite;
    
    private readonly List<Vector2> _points = new();
    
    public bool IsBeingKnockedBack { get; set; }
    
    public Animator _animator;
    public GameObject _shield;
    
    public Rigidbody2D Rigidbody2D => _rigidbody2D;
    public SpriteRenderer SpriteRenderer => _spriteRenderer;
    public float Speed => _speed;
    public float JumpForce => _jumpForce;
    public float AirControl => _airControl;
    public float KnockbackForce => _knockbackHorizontalForce;
    public float KnockbackVerticalForce => _knockbackVerticalForce;
    public List<GameplayAction>Attacks => _attacks;
    public List<GameplayAction> LightAttacks => _lightAttacks;
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
        _nextAction = _attacks.First();
        _nextAction = _lightAttacks.First();
    }

    private void Update() {
        
        Flip();
        
        _currentState.OnUpdate();
        
        
        // Transitions Attack
        if (_currentState is MoveState && _inputController.IsAttacking.Value) {
            SwitchState(new AttackState(gameObject, _nextAction.Speed));
            return;
        }

        if (_currentState is AttackState { IsFinished: true }) {
            SwitchState(new MoveState(gameObject));
            return;
        }

        if (_currentState is MoveState && _inputController.IsLightAttacking.Value)
        {
            SwitchState(new LightAttackState(gameObject, _nextAction.Speed));
            return;
        }
        
        if (_currentState is LightAttackState { IsFinished: true })
        {
            SwitchState(new MoveState(gameObject));
            return;
        }

        if (_currentState is MoveState && _inputController.IsBlocking && IsGrounded)
        {
            SwitchState(new BlockState(gameObject));
        }

        if (_currentState is BlockState && !_inputController.IsBlocking)
        {
            SwitchState(new MoveState(gameObject));
        }
        
        //Hit & Knockback transition
        
        if (_currentState is HitState {IsFinished: true , CausesKnockback: true})
        {
            SwitchState(new KnockUpState(gameObject));
        }

        if (_currentState is HitState { IsFinished: true, CausesKnockback: false })
        {
            SwitchState(new KnockUpState(gameObject));
        }

        if (_currentState is KnockUpState && Rigidbody2D.linearVelocity.y <=0 )
        {
            SwitchState(new KnockUpDownState(gameObject));
        }

        if (_currentState is KnockUpDownState && Rigidbody2D.linearVelocity.y < 0)
        {
            SwitchState(new KnockFallState(gameObject));
        }

        if (_currentState is KnockFallState && IsGrounded)
        {
            SwitchState(new KnockDownState(gameObject));
        }

        if (_currentState is KnockDownState { IsFinished: true })
        {
            SwitchState(new GetUpState(gameObject));
        }

        if (_currentState is GetUpState { IsFinished: true })
        {
            SwitchState(new MoveState(gameObject));
        }

        //Transition Jump
        if (_currentState is MoveState && _inputController.IsJumping && IsGrounded) {
            SwitchState(new JumpState(gameObject));
            return;
        }

        if (_currentState is JumpState && Rigidbody2D.linearVelocityY > 0  )
        {
            SwitchState(new JumpUpState(gameObject));
            return;
        }
        
        if(_currentState is JumpUpState && Rigidbody2D.linearVelocityY < 0)
        { 
           SwitchState(new FallState(gameObject));
        }
         
        if (_currentState is FallState && !_inputController.IsJumping && IsGrounded) {
            SwitchState(new LandState(gameObject, 0.2f));
        }

        if (_currentState is LandState {IsFinished: true})
        {
            SwitchState(new MoveState(gameObject));
        }
    }
    
    private void SwitchState(State state) {
        Debug.Log($"Exiting State : {_currentState?.GetType().Name}");
        _currentState?.OnExit();
        _currentState = state;
        _currentState?.OnEnter();
        Debug.Log($"Entering State : {_currentState?.GetType().Name}");
    }


    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("HurtBox"))
        {
            bool causesKnockback = RegisterHit();
            
           SwitchState(new HitState(gameObject, 0.5f, causesKnockback)); 
            // if (|| _currentState is DeadState) return;
        }
    }

    public bool RegisterHit() {
        if (_lastHitTime < 0f || Time.time - _lastHitTime > _comboResetTime)
        {
            _receivedHitCount = 0;
        }
        _lastHitTime = Time.time;
        _receivedHitCount++;

        if (_receivedHitCount >= 3)
        {
            _receivedHitCount = 0;
            return true;
        }
        return false;
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