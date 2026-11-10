using UnityEngine;

public class BlockState : State
{
    
    private float _timer = 3f;
    public BlockState(GameObject host, float duration = -1) : base(host, duration) { }

    public override void OnEnter() {
        base.OnEnter();
        
        Animator.SetBool("IsBlocking", true);
    }

    public override void OnUpdate() {
        base.OnUpdate();
        
        PlayerController._shield.SetActive(true);

        if (_timer <= 0)
        {
            OnExit();
        }
    }

    public override void OnExit() {
        base.OnExit();
        
        PlayerController._shield.SetActive(false);
        Animator.SetBool("IsBlocking", false);
    }
}