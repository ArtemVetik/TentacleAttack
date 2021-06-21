using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CameraShaker : MonoBehaviour
{
    [SerializeField] private EnemyContainer _enemyContainer;
    [SerializeField] private ObstacleContainer _obstacleContainer;

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        _enemyContainer.EnemyStucked += OnEnemyStucked;
        _obstacleContainer.ObstacleActivated += OnObstacleActivated;
    }

    private void OnDisable()
    {
        _enemyContainer.EnemyStucked -= OnEnemyStucked;
        _obstacleContainer.ObstacleActivated -= OnObstacleActivated;
    }

    private void OnEnemyStucked(Enemy enemy)
    {
        _animator.SetTrigger("Shake");
    }

    private void OnObstacleActivated(Obstacle obstacle)
    {
        _animator.SetTrigger("ExplosionShake");
    }
}
