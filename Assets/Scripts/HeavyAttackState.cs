using UnityEngine;

public class HeavyAttackState : State
{
    public HeavyAttackState(GameObject host, float duration = -1) : base(host, duration) { }
    
    public override void OnEnter() {
        base.OnEnter();
        
        InputController.IsHeavyAttacking.Stop();
        Animator.speed = 1 / Duration;
        Animator.SetBool("IsHeavyAttacking", true);
        // PlayerController.Invoke(nameof(Untint), 0.2f);
    }

    public override void OnExit() {
        base.OnExit();
        
        Animator.speed = 1;
        Animator.SetBool("IsHeavyAttacking", false);
    }

}