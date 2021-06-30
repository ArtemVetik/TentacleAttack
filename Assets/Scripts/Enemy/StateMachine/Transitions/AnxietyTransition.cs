using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnxietyTransition : Transition
{
    private EnemyContainer _enemyContainer;
    private Enemy _selfEnemy;

    private void Awake()
    {
        _selfEnemy = GetComponentInParent<Enemy>();
        _enemyContainer = FindObjectOfType<EnemyContainer>();
    }

    protected override void Enable()
    {
        _enemyContainer.EnemyStucked += OnEnemyStucked;
    }

    private void OnDisable()
    {
        _enemyContainer.EnemyStucked -= OnEnemyStucked;
    }

    private void OnEnemyStucked(Enemy enemy)
    {
        if (enemy.Equals(_selfEnemy))
            return;

        NeedTransit = true;
    }
}
