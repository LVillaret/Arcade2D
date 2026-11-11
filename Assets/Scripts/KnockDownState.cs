using UnityEngine;

public class KnockDownState : State
{
    public KnockDownState(GameObject host, float duration = -1) : base(host, duration) { }

    public override void OnEnter() {
        base.OnEnter();
        Animator.SetBool("KnockDown", true);
    }
    public override void OnExit() {
        base.OnExit();
        Animator.SetBool("KnockDown", false);
    }
}
