/*using UnityEngine;

public class HitState : State
{
    public HitState(GameObject host, float duration = -1) : base(host, duration) { }

    public override void OnEnter() {
        base.OnEnter();
        
        Animator.SetBool("Hit", true);

        PlayerController.Rigidbody2D.linearVelocity =
            PlayerController.KnockbackDirection * PlayerController.KnockbackForce;
    }
    
    

    public override void OnExit() {
        base.OnExit();
        Animator.SetBool("Hit", false);
        
    }
}*/