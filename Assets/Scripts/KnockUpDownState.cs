using UnityEngine;

public class KnockUpDownState : State
{
    public KnockUpDownState(GameObject host, float duration = -1) : base(host, duration) { }

    public override void OnEnter() {
        base.OnEnter();
        Animator.SetBool("KnockUpDown", true);
    }

    public override void OnExit() {
        base.OnExit();
        Animator.SetBool("KnockUpDown", false);
    }
}