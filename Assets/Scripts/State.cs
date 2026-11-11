using UnityEngine;

public abstract class State {
    protected PlayerController PlayerController;
    protected InputController InputController;
    protected Animator Animator;
    protected float Duration;
    private float _timer;

    public bool IsFinished => _timer is > -1 and <= 0f;
    
    protected State(GameObject host, float duration) {
        PlayerController = host.GetComponent<PlayerController>();
        InputController = host.GetComponent<InputController>();
        Animator = host.GetComponent<Animator>();
        Duration = duration;
    }

    public virtual void OnEnter() {
        _timer = Duration;
        //if(Duration > 0f) Animator.speed = 1 / Duration;
        
    }

    public virtual void OnUpdate() {
        if (Duration > 0f) _timer -= Time.deltaTime;
    }
    public virtual void OnExit() {}
}