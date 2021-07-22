using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    [SerializeField] private State _firstState;

    public event UnityAction<Enemy> Stucked;
    public event UnityAction<EnemyTransitState> Transitioned;

    public bool IsStucked => _currentState is StuckState;

    private State _currentState;

    private void Start()
    {
        Transit(_firstState);
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

        ChangeStateEvent(nextState);

        if (_currentState != null)
            _currentState.Enter();
    }

    private void ChangeStateEvent(State nextState)
    {
        if(nextState is AttackState)
        {
            Transitioned?.Invoke(EnemyTransitState.Attack);
        }
        else if(nextState is IdleState)
        {
            Transitioned?.Invoke(EnemyTransitState.Sleep);
        }
        else if(nextState is PatrolState)
        {
            Transitioned?.Invoke(EnemyTransitState.Patrol);
        }

    }

    public void ApplyDamage()
    {
        if (IsStucked)
            return;

        Stucked?.Invoke(this);
    }
}
