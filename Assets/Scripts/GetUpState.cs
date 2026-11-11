using UnityEngine;

public class GetUpState : State
{
    public GetUpState(GameObject host, float duration = -1) : base(host, duration) { }

    public override void OnEnter() {
        base.OnEnter();
        
        Animator.SetBool("IsGettingUp", true);
    }

    public override void OnExit() {
        base.OnExit();
        Animator.SetBool("IsGettingUp", false);
        
        PlayerController.IsBeingKnockedBack = false;
    }
}