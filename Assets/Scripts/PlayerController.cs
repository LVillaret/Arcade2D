
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [SerializeField] private float _speed;
    [SerializeField] private float _airControl;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _groundCheckDistance;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private PolygonCollider2D _hurtBox;
    [SerializeField] private List<GameplayAction> _attacks;
    [SerializeField] private List<GameplayAction> _lightAttacks;
    [SerializeField] private GameplayAction _nextAction;
    [SerializeField] private GameplayAction _nextLightAction;
    [SerializeField] private float _knockbackHorizontalForce;
    [SerializeField] private float _knockbackVerticalForce;
    [SerializeField] private float _maxShield = 100f;
    [SerializeField] private float _shieldDrainPerSecond = 10f;
    [SerializeField] private float _shieldDamagePerHit = 25f;
    [SerializeField] private float _shieldRegenPerSecond = 20f;
    [SerializeField] private float _shieldRegenDelay = 1.5f;
    [SerializeField] private float _shieldMinToBlock = 20f;
    [SerializeField] private float _jumpBufferDuration = 0.5f;
    
    private float _jumpBufferCounter;
    private float _knockbackForce;
    private float _lastPositionX;
    private Vector2 _direction;
    private Rigidbody2D _rigidbody2D;
    private SpriteRenderer _spriteRenderer;
    private State _currentState;
    private InputController _inputController;
    private Sprite _lastSprite;
    
    private readonly List<Vector2> _points = new();
    private float _shieldHealth;
    private float _lastShieldUseTime;
    private Vector3 _shieldBaseScale;
    
    public int currentHealth;
    public int maxHealth;
    public Animator _animator;
    public GameObject _shield;
    
    public float ShieldDrainPerSecond => _shieldDrainPerSecond;
    public Rigidbody2D Rigidbody2D => _rigidbody2D;
    public SpriteRenderer SpriteRenderer => _spriteRenderer;
    public State  CurrentState => _currentState;
    public float Speed => _speed;
    public float JumpForce => _jumpForce;
    public float AirControl => _airControl;
    public List<GameplayAction>Attacks => _attacks;
    public List<GameplayAction> LightAttacks => _lightAttacks;
    public float KnockbackHorizontalForce => _knockbackHorizontalForce;
    public float KnockbackVerticalForce => _knockbackVerticalForce;
    public float KnockbackForce => _knockbackForce;
    public Vector2 KnockbackDirection{ get; private set; }
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

    public GameplayAction NextLightAction
    {
        get => _nextLightAction;
        set => _nextLightAction = value;
    }

    private void Awake() {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _inputController = GetComponent<InputController>();
        
        SwitchState(new MoveState(gameObject));
    }

    private void Start() {
        if (_attacks != null && _attacks.Count > 0)
        {
            _nextAction = _attacks[0];
        }
        else
        {
            Debug.LogError("No attacks have been assigned");
        }

        if (_lightAttacks != null && _lightAttacks.Count > 0)
        {
            _nextLightAction = _lightAttacks[0];
        }
        else
        {
            Debug.LogError("No lightAttacks have been assigned");
        }
        //_nextAction = _attacks.First();
        //_nextAction = _lightAttacks.First();
        currentHealth = maxHealth;
        _shieldHealth = _maxShield;
        _shieldBaseScale = _shield.transform.localScale;
    }

    private void Update() {
        
        Flip();

        if (_inputController.JumpPressed)
        {
            _jumpBufferCounter = _jumpBufferDuration;
            _inputController.ConsumeJumpPressed();
        }
        else if (_currentState is not LandState && _jumpBufferCounter > 0f)
        {
            _jumpBufferCounter -= Time.deltaTime;
        }
        _currentState.OnUpdate();
        UpdateShieldRegeneration();

        if (currentHealth <= 0 && !(_currentState is DeathState))
        {
            SwitchState(new DeathState(gameObject));
            return;
        }
        
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
            if (_nextLightAction == null) return;
            SwitchState(new LightAttackState(gameObject, _nextLightAction.Speed));
            return;
        }
        
        if (_currentState is LightAttackState { IsFinished: true })
        {
            SwitchState(new MoveState(gameObject));
            return;
        }

        if (_currentState is MoveState && _inputController.IsBlocking && IsGrounded && _shieldHealth >= _shieldMinToBlock)
        {
            SwitchState(new BlockState(gameObject));
            return;
        }

        if (_currentState is BlockState && !_inputController.IsBlocking)
        {
            SwitchState(new MoveState(gameObject));
        }
        //Transition Jump
        if (_currentState is MoveState && _jumpBufferCounter > 0f && IsGrounded)
        {
            _jumpBufferCounter = 0f;
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
         
        if (_currentState is FallState &&  IsGrounded) {
            SwitchState(new LandState(gameObject, 0.2f));
        }

        if (_currentState is LandState {IsFinished: true})
        {
            if (_jumpBufferCounter > 0f)
            {
                _jumpBufferCounter = 0f;
                SwitchState(new JumpState(gameObject));
            }
            else
            {
                SwitchState(new MoveState(gameObject));
            }
            return;
        }
        //
        if (_currentState is HitState { IsFinished: true })
        {
            SwitchState(new MoveState(gameObject));
        }
        if (_currentState is StaggerState { IsFinished: true })
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
    
    public bool UseShield(float amount) {
        _shieldHealth -= amount;
        _shieldHealth = Mathf.Max(0, _shieldHealth);
        
        UpdateShieldScale();
        
        return _shieldHealth > 0;
    }

    private void UpdateShieldScale() {
        float ratio = _shieldHealth / _maxShield;
        _shield.transform.localScale = _shieldBaseScale * ratio;
    }

    public void DamageShield() {
        bool shieldAlive = UseShield(_shieldDamagePerHit);
        
        _lastShieldUseTime = Time.time;

        if (!shieldAlive)
        {
            BreakShield();
        }
    }

    public void BreakShield() {
        if (_currentState is BlockState)
        {
            SwitchState(new StaggerState(gameObject, 0.3f));
        }
    }

    public void OnBlockEnded() {
        _lastShieldUseTime =  Time.time;
    }

    private void UpdateShieldRegeneration() {
        if (_currentState is BlockState || _currentState is StaggerState)
        {
            return;
        }

        if (_shieldHealth >_maxShield)
        {
            return;
        }

        if (Time.time < _lastShieldUseTime + _shieldRegenDelay)
        {
            return;
        }
        
        _shieldHealth += _shieldRegenPerSecond *  Time.deltaTime;
        _shieldHealth = Mathf.Min(_shieldHealth, _maxShield);
        
        UpdateShieldScale();
    }


    private void OnTriggerEnter2D(Collider2D other) {
        
        if (currentHealth <= 0) return;
        

        if (!other.CompareTag("HurtBox")) return;
        
        PlayerController attacker = other.transform.parent.GetComponent<PlayerController>();

        if (attacker == null || attacker == this) return;

        if (_currentState is BlockState &&
            (attacker.CurrentState is AttackState || attacker.CurrentState is LightAttackState))
        {
            DamageShield();
            return;
        }
        int damage = 0;

        if (attacker._currentState is AttackState)
        {
            damage = attacker.NextAction.Damage;
        }
        else if (attacker._currentState is LightAttackState)
        {
            damage = attacker.NextLightAction.Damage;
        }

        if (damage <= 0) return;
        
        _knockbackForce = KnockbackHorizontalForce * KnockbackVerticalForce;
        float side = Mathf.Sign(transform.position.x - other.transform.position.x);
        KnockbackDirection = new Vector2(side, 0.4f);
        SwitchState(new HitState(gameObject, damage));

        /*if(other.CompareTag("HurtBox") && currentHealth >= 0)
        {


           ;
            PlayerController attacker = other.transform.parent.GetComponent<PlayerController>();
            int damage = 0;
            if(attacker.CurrentState is AttackState)
            {
                damage = attacker.NextAction.Damage;
            }
            else if (attacker.CurrentState is LightAttackState)
            {
                damage = attacker.NextAction.Damage;
            }
            SwitchState(new HitState(gameObject, damage));

        }
        else if (currentHealth <= 0)
        {
            SwitchState(new DeathState(gameObject));

        }*/
    }
    public void SwitchDeathState()
    {
        SwitchState(new DeathState(gameObject));
        return;
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
        
        if (_currentState is AttackState || _currentState is LightAttackState)
            return;
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