using System;
using System.Collections.Generic;
using UnityEngine;
using SplineMesh;

class EnemyHolder : MonoBehaviour
{
    [SerializeField] private float _startIndent;
    [SerializeField] private float _stepBetweenEnemy;
    [SerializeField] private Transform _tentacleHug;

    private Spline _spline;
    private SplineMovement _movement;
    private TargetDamager _damager;
    private List<Hug> _enemies;
    private const float _correctionFactor = 1.5f;

    private void Start()
    {
        _spline = GetComponent<Spline>();
        _movement = GetComponent<SplineMovement>();
        _damager = GetComponentInChildren<TargetDamager>();

        _enemies = new List<Hug>();

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
        if (!ContainsEnemy(enemy))
        {
            var hugTentacle = Instantiate(_tentacleHug, enemy.transform.position, Quaternion.Euler(-90, 90, 0));
            Hug hug = new Hug(enemy.transform, hugTentacle);
            hug.HugCompleat += RemoveEnemy;
            _enemies.Add(hug);
        }
    }

    private void OnChangePosition()
    {
        for (int i = _enemies.Count - 1; i >= 0; i--)
        {
            int stepNumber = (_enemies.Count - 1) - i;
            float distance = _spline.Length - (_startIndent + _stepBetweenEnemy * i);

            Vector3 position = distance > 0 ? _spline.GetSampleAtDistance(distance).location : _spline.GetSampleAtDistance(0.01f).location;

            if (_enemies[i] != null)
                _enemies[i].SetPosition(position);
        }
    }

    private bool ContainsEnemy(Enemy enemy)
    {
        foreach(var savedEnemy in _enemies)
        {
            if(savedEnemy.Equals(enemy))
            {
                return true;
            }
        }

        return false;
    }

    private void RemoveEnemy(Hug hug)
    {
        _enemies.Remove(hug);
    }

    private class Hug
    {
        private Transform _enemy;
        private Transform _hug;

        public event Action<Hug> HugCompleat;

        public Hug (Transform enemy, Transform hug)
        {
            _enemy = enemy;
            _hug = hug;
        }

        public void SetPosition(Vector3 position)
        {
            Vector3 enemyPosition = position;
            enemyPosition.y -= _correctionFactor;

            if (_enemy != null)
            {
                _enemy.position = enemyPosition;
                _hug.position = position;
            }
            else
            {
                Destroy(_hug.gameObject);
                HugCompleat?.Invoke(this);
            }
        }

        public bool Equals(Enemy enemy)
        {
            return _enemy.GetComponent<Enemy>().Equals(enemy);
        }
    }

}


