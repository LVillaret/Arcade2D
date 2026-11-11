using UnityEngine;

public class HitState : State
{
    private Color _baseColor;
    
    public bool CausesKnockback { get; private set; }
    public HitState(GameObject host, float duration = -1, bool causesKnockback = false) : base(host, duration) { CausesKnockback = causesKnockback; }

    public override void OnEnter() {
        base.OnEnter();
        
        _baseColor = PlayerController.SpriteRenderer.color;
        PlayerController.SpriteRenderer.color = Color.red;
        // PlayerController.Rigidbody2D.linearVelocity = PlayerController.KnockbackDirection * PlayerController.KnockbackForce;
        Animator.SetBool("Hit", true);

        if (CausesKnockback)
        {
            PlayerController.IsBeingKnockedBack = true;
        }
        // PlayerController.Rigidbody2D.linearVelocity =
            //PlayerController.KnockbackDirection * PlayerController.KnockbackForce;
    }
    
    

    public override void OnExit() {
        base.OnExit();
        
        PlayerController.SpriteRenderer.color = _baseColor;
        Animator.SetBool("Hit", false);
        
    }
}