using UnityEngine;

public class KnockFallState : State
{
    public KnockFallState(GameObject host, float duration = -1) : base(host, duration) { }

    public override void OnEnter() {
        base.OnEnter();
        
        Animator.SetBool("KnockFall", true);
    }

    public override void OnExit() {
        base.OnExit();
        Animator.SetBool("KnockFall", false);
    }
}