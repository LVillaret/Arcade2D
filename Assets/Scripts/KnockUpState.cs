using UnityEngine;

public class KnockUpState : State
{
    public KnockUpState(GameObject host, float duration = -1) : base(host, duration) { }

    public override void OnEnter() {
        base.OnEnter();
        Animator.SetBool("KnockUp", true);

        float _direction = PlayerController.transform.localScale.x >= 0
            ? -1f
            : 1f;
        PlayerController.Rigidbody2D.linearVelocity = new Vector2(_direction * PlayerController.KnockbackForce, PlayerController.KnockbackVerticalForce);
    }

    public override void OnExit() {
        base.OnExit();
        Animator.SetBool("KnockUp", false);
    }
}