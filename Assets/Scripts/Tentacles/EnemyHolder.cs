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

    public event Action EnemyHold;
    public event Action EnemyLeave;

    private void Awake()
    {
        _spline = GetComponent<Spline>();
        _movement = GetComponent<SplineMovement>();
        _damager = GetComponentInChildren<TargetDamager>();
    }

    private void Start()
    {
        _enemies = new List<Hug>();
    }

    private void OnEnable()
    {
        GlobalEventStorage.TentacleAddDamageAddListener(OnTentacleDamaged);
        _damager.EnemyFounded += AddEnemy;
        _movement.SplineChanged += OnChangePosition;
    }

    private void Update() { }

    private void OnDisable()
    {
        GlobalEventStorage.TentacleAddDamageRemoveListener(OnTentacleDamaged);
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
            EnemyHold?.Invoke();
        }
    }

    private void OnChangePosition()
    {
        for (int i = _enemies.Count - 1; i >= 0; i--)
        {
            int stepNumber = (_enemies.Count - 1) - i;
            float distance = _spline.Length - (_startIndent + _stepBetweenEnemy * i);

            var sample = distance > 0 ? _spline.GetSampleAtDistance(distance) : _spline.GetSampleAtDistance(0.01f);

            if (_enemies[i] != null)
                _enemies[i].SetTransform(sample);
        }
    }

    private void OnTentacleDamaged()
    {
        foreach (var enemy in _enemies)
        {
            var ragdollTemplate = enemy.RagdollModel;
            var inst = ragdollTemplate.InstRagdollEnemy(enemy.EnemyPosition, enemy.EnemyRotation);
            inst.EnableRagdoll();
            enemy.DestroyWithEnemy();
        }
    }

    private bool ContainsEnemy(Enemy enemy)
    {
        foreach (var savedEnemy in _enemies)
        {
            if (savedEnemy.Equals(enemy))
            {
                return true;
            }
        }

        return false;
    }

    private void RemoveEnemy(Hug hug)
    {
        _enemies.Remove(hug);
        EnemyLeave?.Invoke();
    }

    private class Hug
    {
        private Transform _enemy;
        private Transform _hug;

        public event Action<Hug> HugCompleat;

        public Vector3 EnemyPosition => _enemy.position;
        public Quaternion EnemyRotation => _enemy.rotation;
        public RagdollContainer RagdollModel => _enemy.GetComponent<RagdollContainer>();

        public Hug(Transform enemy, Transform hug)
        {
            _enemy = enemy;
            _hug = hug;
        }

        public void SetTransform(CurveSample sample)
        {
            Vector3 enemyPosition = sample.location;

            if (_enemy != null)
            {
                _enemy.position = enemyPosition;
                _enemy.localPosition -= _enemy.up * _correctionFactor;
                _hug.position = sample.location;
                _enemy.rotation = sample.Rotation;
                _hug.rotation = sample.Rotation;
            }
            else
            {
                Destroy(_hug.gameObject);
                HugCompleat?.Invoke(this);
            }
        }

        public bool Equals(Enemy enemy)
        {
            if (enemy != null)
                return _enemy.GetComponent<Enemy>().Equals(enemy);
            return false;
        }

        public void DestroyWithEnemy()
        {
            Destroy(_hug.gameObject);
            Destroy(_enemy.gameObject);
        }
    }

}


