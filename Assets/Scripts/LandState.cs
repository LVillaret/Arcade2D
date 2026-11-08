using UnityEngine;

public class LandState : State{
    public LandState(GameObject host, float duration = -1) : base(host, duration) { }
    
    
    public override void OnEnter() {
        base.OnEnter();
        Animator.SetBool("IsFalling", true);
    }
    

    public override void OnExit() {
        base.OnExit();
        
        Animator.SetBool("IsFalling", false);
    }
}