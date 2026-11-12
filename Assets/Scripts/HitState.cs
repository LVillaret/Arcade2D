using UnityEngine;

public class HitState : State
{
    
    public Color _baseColor;

    public HitState(GameObject host, int damage, float duration = -1) : base(host, duration) {
        _damage = damage;
    }

    private int _damage;
    public override void OnEnter() {
        base.OnEnter();
        
        _baseColor = PlayerController.SpriteRenderer.color;
        PlayerController.SpriteRenderer.color = Color.red;
        
        
        
        PlayerController.currentHealth -= _damage;
        
        Debug.Log(PlayerController.currentHealth);
        //Animator.SetBool("Hit", true);
        PlayerController.Rigidbody2D.linearVelocity = 
            PlayerController.KnockbackDirection * PlayerController.KnockbackForce;
        //PlayerController.Rigidbody2D.linearVelocity =
           // PlayerController.KnockbackDirection * PlayerController.KnockbackForce;
    }
    
    

    public override void OnExit() {
        base.OnExit();
        PlayerController.SpriteRenderer.color = _baseColor;
        //Animator.SetBool("Hit", false);
        
    }
}