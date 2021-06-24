using System;
using System.Collections.Generic;
using UnityEngine;
using SplineMesh;

class EnemyHolder : MonoBehaviour
{
    [SerializeField] private float _startIndent;
    [SerializeField] private float _stepBetweenEnemy;

    private Spline _spline;
    private SplineMovement _movement;

    private TargetDamager _damager;

    private List<Enemy> _enemies;

    private void Start()
    {
        _spline = GetComponent<Spline>();
        _movement = GetComponent<SplineMovement>();
        _damager = GetComponentInChildren<TargetDamager>();

        _enemies = new List<Enemy>();

        _damager.EnemyFounded += AddEnemy;
        _movement.SplineChanged += OnChangePosition;
    }

    private void OnDisable()
    {
        _damager.EnemyFounded -= AddEnemy;
        _movement.SplineChanged -= OnChangePosition;
    }

    public void AddEnemy(Enemy enemy)
    {
        _enemies.Add(enemy);
    }

    private void OnChangePosition()
    {
        for(int i = _enemies.Count - 1; i >= 0; i--)
        {
            int stepNumber = (_enemies.Count - 1) - i;
            float distance = _spline.Length - (_startIndent + _stepBetweenEnemy * i);

            Vector3 position = distance > 0 ? _spline.GetSampleAtDistance(distance).location : _spline.GetSampleAtDistance(0.01f).location;
            position.y -= 1.0f;
            _enemies[i].transform.position = position;
        }
    }
}

