using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyDetecter : MonoBehaviour
{
    [SerializeField] private EnemyDetectorIcon _enemyDetecterIcon;
    [SerializeField] private EnemyContainer _enemyContainer;

    private List<Detecter> _detecters;

    private void Start()
    {
        _enemyContainer.EnemyStucked += OnEnemyStickeed;
        _detecters = new List<Detecter>();

        for (int i = 0; i < _enemyContainer.AliveEnemyCount; i++)
        {
            Enemy enemy = _enemyContainer.GetEnemy(i);
            Detecter detecter = new Detecter(enemy, Instantiate(_enemyDetecterIcon, this.transform));

            _detecters.Add(detecter);
        }
    }

    private void OnDisable()
    {
        _enemyContainer.EnemyStucked -= OnEnemyStickeed;
    }

    private void Update()
    {
        foreach (var detecter in _detecters)
        {
            if (detecter != null && detecter.IsEnemyLife)
                detecter.UpdateDetecterPosition();
        }
    }

    private void OnEnemyStickeed(Enemy enemy)
    {
        var enemyDetecter = _detecters.Where(detecter => detecter.Equals(enemy)).FirstOrDefault();

        if (enemyDetecter != null)
            _detecters.Remove(enemyDetecter);
    }

    private class Detecter
    {
        private Transform _enemyTransform;
        private EnemyDetectorIcon _detecter;

        private readonly Vector2 _rectOffset = new Vector2(100, 100);

        public bool IsEnemyLife => _enemyTransform != null;

        public Detecter(Enemy enemy, EnemyDetectorIcon detecter)
        {
            _enemyTransform = enemy.transform;
            _detecter = detecter;

            enemy.Transitioned += TransiteState;
            enemy.Stucked += OnEnemyStucked;
        }

        public bool Equals(Enemy enemy)
        {
            return _enemyTransform.Equals(enemy.transform);
        }

        public void UpdateDetecterPosition()
        {
            Vector3 iconPosition = _enemyTransform.position;
            iconPosition.y += 2;

            Vector3 screenPosition = Camera.main.WorldToScreenPoint(iconPosition);
            Rect rect = Camera.main.pixelRect;

            _detecter.transform.position = new Vector3(Mathf.Clamp(screenPosition.x, rect.xMin + _rectOffset.x, rect.xMax - _rectOffset.x),
                Mathf.Clamp(screenPosition.y, rect.yMin + _rectOffset.y, rect.yMax - _rectOffset.y));
        }

        private void TransiteState(EnemyTransitState state)
        {
            _detecter.ChangeIcon(state); 
        }

        private void OnEnemyStucked(Enemy enemy)
        {
            enemy.Transitioned -= TransiteState;
            enemy.Stucked -= OnEnemyStucked;
            Destroy(_detecter.gameObject);
        }
    }
}