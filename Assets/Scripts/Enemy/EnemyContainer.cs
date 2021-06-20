using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class EnemyContainer : MonoBehaviour
{
    public event UnityAction<Enemy> EnemyStucked;

    private List<Enemy> _allEnemies;

    private void Awake()
    {
        _allEnemies = FindObjectsOfType<Enemy>().ToList();
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
        EnemyStucked?.Invoke(enemy);
    }
}
