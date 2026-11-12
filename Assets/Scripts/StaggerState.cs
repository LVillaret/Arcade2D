using UnityEngine;

public class StaggerState : State
{
    public StaggerState(GameObject host, float duration = -1) : base(host, duration) { }

    public override void OnEnter() {
        base.OnEnter();
        Animator.SetBool("Stagger", true);
    }

    public override void OnExit() {
        base.OnExit();
        Animator.SetBool("Stagger", false);
    }
}