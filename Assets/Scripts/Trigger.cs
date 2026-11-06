using UnityEngine;

public struct Trigger
{
    [SerializeField] private float _endTime;

    private bool _hasBeenTriggered;
    private const float Duration = 0.3f;

    public bool Value => Time.time < _endTime && _hasBeenTriggered;

    public void Launch()
    {
        _endTime = Time.time + Duration;
        _hasBeenTriggered = true;
    }

    public void Stop() {
        _endTime = Time.time + Duration;
    }
}