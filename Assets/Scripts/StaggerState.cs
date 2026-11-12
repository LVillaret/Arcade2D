using UnityEngine;

public class StaggerState : State
{
    public StaggerState(GameObject host, float duration = 0.3f) : base(host, duration) {
    }

    public override void OnEnter() {
        base.OnEnter();
        Animator.SetBool("Stagger", true);
        PlayerController.Rigidbody2D.linearVelocity = new Vector2(0, PlayerController.Rigidbody2D.linearVelocity.y);
    }

    public override void OnExit() {
        base.OnExit();
        Animator.SetBool("Stagger", false);
    }
}