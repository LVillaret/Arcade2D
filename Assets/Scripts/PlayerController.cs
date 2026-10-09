using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _groundCheckDistance;
    [SerializeField] private LayerMask _groundLayer;
    
    private Rigidbody2D _rigidbody2D;
    private State _currentState;
    private Vector2 _move;
    private bool _isJumping;
    private bool _isAttacking;
    
    
    public Rigidbody2D Rigidbody2D => _rigidbody2D;
    public Vector2 Move => _move;
    public float Speed => _speed;
    public float JumpForce => _jumpForce;

    public bool IsGrounded
    {
        get
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, _groundCheckDistance, _groundLayer)
            return hit.collider != null;
        }
    }

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        
        SwitchState(new MoveState(playerController : this));
        
    }

    private void Update()
    {
        _currentState.OnUpdate();

        if (_currentState is MoveState && _isJumping && IsGrounded)
        {
            SwitchState(new JumpState(playerController : this));
        }

        if (_currentState is JumpState && !_isJumping && IsGrounded)
        {
            SwitchState(new MoveState(playerController : this));
        }

        if (_currentState is MoveState && _isAttacking && IsGrounded)
        {
            SwitchState(new AttackState());
        }
    }
    
    private void SwitchState(State state) {
        _currentState?.OnExit();
        _currentState = state;
        _currentState.OnEnter();
    }
    public void OnMove(InputValue value)
    {
       _move = value.Get<Vector2>();
    }

    public void OnJump(InputValue value) {
        _isJumping = value.isPressed;
    }

    public void OnAttacking(InputValue value) {
        _isAttacking = value.isPressed;
    }
}