using UnityEngine;

public class JumpState : State {
    public JumpState(GameObject host, float duration = -1) : base(host, duration) { }
 
    public override void OnEnter() {
        base.OnEnter();
        
        PlayerController._animator.SetFloat("IsWalking", 0f);
        Animator.SetBool("IsJumping", true);
        PlayerController.Rigidbody2D.AddForce(Vector2.up * PlayerController.JumpForce,  ForceMode2D.Impulse);
    }

    public override void OnUpdate() {
        base.OnUpdate();
        
        Animator.SetBool("IsJumping", true);
        
        float velocity = InputController.Move.x * (PlayerController.AirControl * Time.deltaTime);
        PlayerController.Rigidbody2D.linearVelocity = new Vector2(velocity, PlayerController.Rigidbody2D.linearVelocity.y);
    }

    public override void OnExit() {
        base.OnExit();

    }
        
}