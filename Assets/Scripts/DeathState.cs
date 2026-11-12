using UnityEngine;

public class DeathState : State
{
    public DeathState(GameObject host, float duration = -1) : base(host, duration) { }

    public override void OnEnter() {
        base.OnEnter();
        
        Animator.SetBool("Death", true);
    }

    public override void OnExit() {
        base.OnExit();
    }
}