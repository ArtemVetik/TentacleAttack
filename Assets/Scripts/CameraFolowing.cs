using UnityEngine;
using System.Collections;

public class CameraFolowing : MonoBehaviour
{
    [SerializeField] private Transform _followTarget;
    [SerializeField] private StartMovementTrigger _startTrigger;
    [SerializeField] private float _targetPositionZ = -16;
    [Header("Не обязательно")]
    [SerializeField] private Transform _endGamePoint;
    [SerializeField] private Transform _krakenChild;

    private Vector3 _startPosition;
    private bool _started = false;
    private float _speed = 5f;

    private void OnEnable()
    {
        _startTrigger.MoveStarted += OnMoveStarted;
        GlobalEventStorage.GameEnded += OnGameEnded;
        GlobalEventStorage.EnemyEnded += OnEnemyEnded;
    }

    private void OnDisable()
    {
        _startTrigger.MoveStarted -= OnMoveStarted;
        GlobalEventStorage.GameEnded -= OnGameEnded;
        GlobalEventStorage.EnemyEnded -= OnEnemyEnded;
    }

    private void Start()
    {
        _startPosition = transform.position;
    }

    private void OnMoveStarted()
    {
        _started = true;
    }

    private void Update()
    {
        if (_started == false)
            return;

        var nextPosition = Vector3.Lerp(transform.position, _followTarget.position, _speed * Time.deltaTime);
        nextPosition.z = Mathf.Lerp(transform.position.z, _targetPositionZ, _speed * Time.deltaTime);

        transform.position = nextPosition;
    }

    public void ChangeZPosition(float zPosition)
    {
        _targetPositionZ = zPosition;
    }

    public void OnGameEnded(bool isWin, int progress)
    {
        if (isWin == false)
            return;

        _speed *= 0.5f;

        if (_endGamePoint != null)
        {
            if (_krakenChild != null)
                StartCoroutine(ZoomChildCoroutine());
        }
        else
        {
            var kraken = FindObjectOfType<KrakenAnimations>();
            _followTarget = kraken.transform;
            _targetPositionZ = -20;
        }
    }

    public void OnEnemyEnded()
    {
        var target = new GameObject();
        target.transform.position = _startPosition;
        _followTarget = target.transform;
        _targetPositionZ = target.transform.position.z;
    }

    private IEnumerator ZoomChildCoroutine()
    {
        if (_krakenChild != null)
        {
            _followTarget = _krakenChild;
            _targetPositionZ = -15;

            yield return new WaitForSeconds(2f);
            
            _followTarget = _endGamePoint;
            _targetPositionZ = _endGamePoint.position.z;
        }
        else
        {
            _followTarget = _endGamePoint;
            _targetPositionZ = _endGamePoint.position.z;
        }
    }
}
