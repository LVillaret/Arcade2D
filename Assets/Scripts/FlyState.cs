using UnityEngine;

public class FlyState : State
{
    public FlyState(GameObject host, float duration = -1) : base(host, duration) { }


    public override void OnEnter() {
        base.OnEnter();
        
        //Animator.SetBool("IsFlying", true);
    }

    public override void OnUpdate() {
        base.OnUpdate();
        
        float velocity = InputController.Move.x * (PlayerController.AirControl * Time.deltaTime);
        PlayerController.Rigidbody2D.linearVelocity = new Vector2(velocity, PlayerController.Rigidbody2D.linearVelocity.y);
    }

    public override void OnExit() {
        base.OnExit();
        
        Animator.SetBool("IsJumping", false);
        //Animator.SetBool("IsFlying", false);
    }
}