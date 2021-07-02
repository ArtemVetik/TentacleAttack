using UnityEngine;

public class CameraFolowing : MonoBehaviour
{
    [SerializeField] private Transform _followTarget;
    [SerializeField] private StartMovementTrigger _startTrigger;
    [SerializeField] private float _targetPositionZ = -16;

    private bool _started = false;

    private void OnEnable()
    {
        _startTrigger.MoveStarted += OnMoveStarted;
    }

    private void OnDisable()
    {
        _startTrigger.MoveStarted -= OnMoveStarted;
    }

    private void OnMoveStarted()
    {
        _started = true;
    }

    private void LateUpdate()
    {
        if (_started == false)
            return;

        var nextPosition = Vector3.Lerp(transform.position, _followTarget.position, 5f * Time.deltaTime);
        nextPosition.z = Mathf.Lerp(transform.position.z, _targetPositionZ, 5f * Time.deltaTime);

        transform.position = nextPosition;
    }
}
