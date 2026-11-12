using UnityEngine;

public class LightAttackState : State
{
    
    public GameplayAction _currentAction;
    public LightAttackState(GameObject host, float duration = -1) : base(host, duration) { }
    
    public override void OnEnter() {
        base.OnEnter();
        
        _currentAction = PlayerController.NextLightAction;
        InputController.IsLightAttacking.Stop();
        Animator.runtimeAnimatorController = _currentAction.animatorOverrideController;
        Animator.SetBool("IsLightAttacking", true);
        // PlayerController.Invoke(nameof(Untint), 0.2f);
    }

    public override void OnExit() {
        base.OnExit();
        
        int index = PlayerController.LightAttacks.IndexOf(PlayerController.NextLightAction) +1;
        if (index >= PlayerController.LightAttacks.Count) index = 0;
        PlayerController.NextLightAction = PlayerController.LightAttacks[index];
        Animator.SetBool("IsLightAttacking", false);
    }

}