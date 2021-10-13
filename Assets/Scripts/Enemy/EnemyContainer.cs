using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class EnemyContainer : MonoBehaviour
{
    public int AliveEnemyCount { get; private set; }
    public int StartEnemyCount { get; private set; }

    public event UnityAction<Enemy> EnemyStucked;
    public event UnityAction EnemyEnded;

    private List<Enemy> _allEnemies;

    private void Awake()
    {
        _allEnemies = FindObjectsOfType<Enemy>().ToList();
        AliveEnemyCount = _allEnemies.Count;
        StartEnemyCount = _allEnemies.Count;

        foreach (var enemy in _allEnemies)
            enemy.Stucked += OnEnemyStucked;
    }

    private void OnDisable()
    {
        foreach (var enemy in _allEnemies)
            enemy.Stucked -= OnEnemyStucked;
    }

    public Enemy GetEnemy(int index)
    {
        if (index < 0)
        {
            index = 0;
            Debug.LogError("AllEnemy index less than zero");
        }
        else if (index >= _allEnemies.Count)
        {
            index = _allEnemies.Count - 1;
            Debug.LogError("AllEnemy index is greater zen array lenght");
        }

        return _allEnemies[index];
    }

    private void OnEnemyStucked(Enemy enemy)
    {
        AliveEnemyCount--;
        EnemyStucked?.Invoke(enemy);

        if (_allEnemies != null)
            _allEnemies.Remove(enemy);

        if(AliveEnemyCount == 0)
        {
            EnemyEnded?.Invoke();
            GlobalEventStorage.EnemyEndedInvoke();
        }
    }
}
