using UnityEngine;

public class CameraFolowing : MonoBehaviour
{
    [SerializeField] private Transform _followTarget;
    [SerializeField] private StartMovementTrigger _startTrigger;
    [SerializeField] private float _targetPositionZ = -16;
    [SerializeField] private Transform _endGamePoint;

    private bool _started = false;

    private void OnEnable()
    {
        _startTrigger.MoveStarted += OnMoveStarted;
        GlobalEventStorage.GameEnded += OnGameEnded;
    }

    private void OnDisable()
    {
        _startTrigger.MoveStarted -= OnMoveStarted;
        GlobalEventStorage.GameEnded -= OnGameEnded;
    }

    private void OnMoveStarted()
    {
        _started = true;
    }

    private void Update()
    {
        if (_started == false)
            return;

        var nextPosition = Vector3.Lerp(transform.position, _followTarget.position, 5f * Time.deltaTime);
        nextPosition.z = Mathf.Lerp(transform.position.z, _targetPositionZ, 5f * Time.deltaTime);

        transform.position = nextPosition;
    }

    public void ChangeZPosition(float zPosition)
    {
        _targetPositionZ = zPosition;
    }

    public void OnGameEnded(bool isWin)
    {
        if(_endGamePoint != null && isWin)
        {
            _followTarget = _endGamePoint;
            _targetPositionZ = 0.0f;
        }
    }
}
