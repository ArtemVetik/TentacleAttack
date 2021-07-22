using UnityEngine;
using UnityEngine.Events;

public class CompleteLevelTrigger : MonoBehaviour
{
    [SerializeField] private EnemyContainer _enemyContainer;

    public event UnityAction LevelCompleted;

    private void OnEnable()
    {
        _enemyContainer.EnemyStucked += OnEnemyStucked;
    }

    private void OnDisable()
    {
        _enemyContainer.EnemyStucked -= OnEnemyStucked;
    }

    private void OnEnemyStucked(Enemy enemy)
    {
        if (_enemyContainer.AliveEnemyCount == 0)
            LevelCompleted?.Invoke();
    }
}
