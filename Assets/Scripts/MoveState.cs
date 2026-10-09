using UnityEngine;

public class MoveState : State
{
    public MoveState(PlayerController playerController) : base(playerController) { }

    public override void OnEnter() {
        PlayerController.Rigidbody2D.AddForce(Vector2.up * PlayerController.JumpForce, ForceMode2D.Impulse);
    }

    public override void OnUpdate()
    {
        PlayerController.transform.Translate(PlayerController.Move * (Time.deltaTime * PlayerController.Speed));
    }
    
    public override void OnExit() { }
}

public class AttackState : State
{
    public AttackState(PlayerController playerController) : base(playerController) {
    }

    public override void OnEnter() {
        PlayerController.SpriteRenderer.color = Color.red;
        PlayerController.Invoke(nameof(Unint), 0.2f);
    }

    public void Unint() {
        PlayerController.SpriteRenderer.color = Color.white
    }
}