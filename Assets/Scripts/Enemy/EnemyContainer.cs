using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class EnemyContainer : MonoBehaviour
{
    public int AliveEnemyCount { get; private set; }

    public event UnityAction<Enemy> EnemyStucked;
    public event UnityAction EnemyEnded;

    private List<Enemy> _allEnemies;

    private void Awake()
    {
        _allEnemies = FindObjectsOfType<Enemy>().ToList();
        AliveEnemyCount = _allEnemies.Count;
    }

    private void OnEnable()
    {
        foreach (var enemy in _allEnemies)
            enemy.Stucked += OnEnemyStucked;
    }

    private void OnDisable()
    {
        foreach (var enemy in _allEnemies)
            enemy.Stucked -= OnEnemyStucked;
    }

    private void OnEnemyStucked(Enemy enemy)
    {
        AliveEnemyCount--;
        EnemyStucked?.Invoke(enemy);

        if(AliveEnemyCount == 0)
        {
            EnemyEnded?.Invoke();
        }
    }
}
