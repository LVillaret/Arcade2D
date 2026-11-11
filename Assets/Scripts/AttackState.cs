using UnityEngine;

public class AttackState : State
{

    public GameplayAction _currentAction;
    public AttackState(GameObject host, float duration = -1) : base(host, duration) { }

    public override void OnEnter() {
        base.OnEnter();
        
       _currentAction = PlayerController.NextAction;
        
        InputController.IsAttacking.Stop();
        Animator.runtimeAnimatorController = _currentAction.animatorOverrideController;
        Animator.speed = 1 / Duration;
        Animator.SetBool("IsAttacking", true);
        // PlayerController.Invoke(nameof(Untint), 0.2f);

       
       
    }

    public override void OnExit() {
        base.OnExit();
        
        int index = PlayerController.Attacks.IndexOf(PlayerController.NextAction) +1;
        if (index >= PlayerController.Attacks.Count) index = 0;
        PlayerController.NextAction = PlayerController.Attacks[index];
        Animator.speed = 1;
        Animator.SetBool("IsAttacking", false);
    }
    
}