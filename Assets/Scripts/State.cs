using UnityEngine;
using UnityEngine.Rendering.UI;

public abstract class State 
{
    
    protected PlayerController PlayerController;
    private float _duration;
    private float _timer;
    
    public bool IsFinished => _timer > -1 && _timer < _duration;

    protected State(PlayerController playerController, float duration)
    {
        PlayerController = playerController;
        _duration = duration;
    }

    public virtual void OnEnter() {
        _timer = _duration;
    }

    public virtual void OnUpdate() {
        _timer -= Time.deltaTime;
    }
    public virtual void OnExit() { }
}