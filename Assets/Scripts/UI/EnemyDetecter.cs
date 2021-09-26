using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyDetecter : MonoBehaviour
{
    [SerializeField] private EnemyDetectorIcon _enemyDetecterIcon;

    private EnemyContainer _enemyContainer;
    private List<Detecter> _detecters;
    private Transform _target;
    [SerializeField] private Vector3 _vectorUp;

    private void Awake()
    {
        _enemyContainer = FindObjectOfType<EnemyContainer>();
    }

    private void OnEnable()
    {
        _enemyContainer.EnemyStucked += OnEnemyStickeed;
    }

    private void Start()
    {
        _detecters = new List<Detecter>();
        _target = FindObjectOfType<TargetMovement>().transform;

        for (int i = 0; i < _enemyContainer.AliveEnemyCount; i++)
        {
            Enemy enemy = _enemyContainer.GetEnemy(i);
            Detecter detecter = new Detecter(enemy, Instantiate(_enemyDetecterIcon, this.transform), this);
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
        private EnemyDetecter _parentDetector;

        private readonly Vector2 _rectOffset = new Vector2(50, 50);

        public bool IsEnemyLife => _enemyTransform != null;

        public Detecter(Enemy enemy, EnemyDetectorIcon detecter, EnemyDetecter parentDetector)
        {
            _enemyTransform = enemy.transform;
            _detecter = detecter;
            _parentDetector = parentDetector;

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
            Vector3 targetPosition = Camera.main.WorldToScreenPoint(_parentDetector._target.position);
            Rect rect = Camera.main.pixelRect;

            _detecter.gameObject.SetActive(!rect.Contains(screenPosition));

            if (_detecter.gameObject.activeSelf)
            {
                _detecter.gameObject.SetActive(true);
                _detecter.transform.position = new Vector3(Mathf.Clamp(screenPosition.x, rect.xMin + _rectOffset.x, rect.xMax - _rectOffset.x),
                    Mathf.Clamp(screenPosition.y, rect.yMin + _rectOffset.y, rect.yMax - _rectOffset.y));

                Vector3 direction = (screenPosition - targetPosition).normalized;
                float angle = Vector2.SignedAngle(Vector2.right, direction);

                _detecter.transform.eulerAngles = Vector3.forward * angle;
            }
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