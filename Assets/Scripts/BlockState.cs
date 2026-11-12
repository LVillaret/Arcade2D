using UnityEngine;

public class BlockState : State
{
    public BlockState(GameObject host, float duration = -1) : base(host, duration) { }

    public override void OnEnter() {
        base.OnEnter();
        
        Animator.SetBool("IsBlocking", true);
        PlayerController._shield.SetActive(true);
    }

    public override void OnUpdate() {
        base.OnUpdate();
        
        bool shieldActive = PlayerController.UseShield(PlayerController.ShieldDrainPerSecond * Time.deltaTime);

        if (!shieldActive)
        {
            PlayerController.BreakShield();
        }
    }

    public override void OnExit() {
        base.OnExit();
        
        PlayerController._shield.SetActive(false);
        Animator.SetBool("IsBlocking", false);

        PlayerController.OnBlockEnded();
    }
}