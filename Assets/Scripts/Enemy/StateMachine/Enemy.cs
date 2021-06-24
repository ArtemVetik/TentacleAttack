using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    [SerializeField] private State _firstState;

    public event UnityAction<Enemy> Stucked;

    public bool IsStucked => _currentState is StuckState;

    private State _currentState;

    private void Start()
    {
        _currentState = _firstState;
        _firstState.Enter();
    }

    private void Update()
    {
        if (_currentState == null)
            return;

        var nextState = _currentState.GetNextState();
        if (nextState != null)
            Transit(nextState);
    }

    private void Transit(State nextState)
    {
        if (_currentState != null)
            _currentState.Exit();

        _currentState = nextState;

        if (_currentState != null)
            _currentState.Enter();
    }

    public void ApplyDamage()
    {
        if (IsStucked)
            return;

        Stucked?.Invoke(this);
    }
}
