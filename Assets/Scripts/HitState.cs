using UnityEngine;

public class HitState : State
{
    //public Color _baseColor;
    private int _damage;

    public HitState(GameObject host, int damage, float duration = 0.5f) : base(host, duration) {
        _damage = damage;
    }
    public override void OnEnter() {
        base.OnEnter();
        
       // _baseColor = PlayerController.SpriteRenderer.color;
       // PlayerController.SpriteRenderer.color = Color.red;
        PlayerController.currentHealth = Mathf.Max(0, PlayerController.currentHealth - _damage);
        
        Debug.Log("Dégâts reçus : " + _damage);
        Debug.Log("Vie restante : " + PlayerController.currentHealth);
        
        PlayerController.Rigidbody2D.linearVelocity = 
            PlayerController.KnockbackDirection * PlayerController.KnockbackForce;

        if (PlayerController.currentHealth <= 0)
        {
            PlayerController.SwitchDeathState();
        }
    }

    public override void OnUpdate() {
        base.OnUpdate();
    }

    public override void OnExit() {
        base.OnExit();
        //PlayerController.SpriteRenderer.color = _baseColor;
        //Animator.SetBool("Hit", false);
        
    }
}