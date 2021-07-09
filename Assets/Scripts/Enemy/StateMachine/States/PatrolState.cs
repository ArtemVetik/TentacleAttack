using RootMotion.FinalIK;
using SplineMesh;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : State
{
    private enum Direction : int
    {
        Left = -1,
        Right = 1,
    };

    [SerializeField] private ViewZoneDetector _viewZone;
    [SerializeField] private float _speed;

    private EnemyAnimations _enemyAnimations;
    private Spline _moveSpline;
    private Quaternion _targetRotation;
    private float _distanceCovered;
    private Direction _directon;
    private bool _rotating;

    private void Awake()
    {
        _moveSpline = GetComponentInParent<Spline>();
        _enemyAnimations = GetComponentInChildren<EnemyAnimations>();
    }

    private void OnEnable()
    {
        _viewZone.Enable();
        _distanceCovered = SplineDistanceByPosition(transform.position);
        _directon = transform.eulerAngles.y > 180 ? Direction.Right : Direction.Left;
        _targetRotation = GetRotationByDirection(_directon);
        _rotating = false;
    }

    private void Update()
    {
        if (_rotating)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, _targetRotation, 100f * Time.deltaTime);

            if (Mathf.Abs(Quaternion.Dot(transform.rotation, _targetRotation)) < 0.999f)
                return;

            _enemyAnimations.StopRotating();
            _rotating = false;
        }

        _distanceCovered += _speed * Time.deltaTime * (int)_directon;

        if (_distanceCovered > _moveSpline.Length || _distanceCovered < 0)
        {
            _distanceCovered = Mathf.Clamp(_distanceCovered, 0, _moveSpline.Length);
            _directon = _directon == Direction.Left ? Direction.Right : Direction.Left;
            _targetRotation = GetRotationByDirection(_directon);
            _rotating = true;
            _enemyAnimations.PlayRotating();
            return;
        }

        var sample = _moveSpline.GetSampleAtDistance(_distanceCovered);

        transform.position = sample.location;

        if (_directon == Direction.Right)
            transform.rotation = sample.Rotation;
        else
            transform.rotation = Quaternion.Euler(-sample.Rotation.eulerAngles.x, -sample.Rotation.eulerAngles.y, sample.Rotation.eulerAngles.z);
    }

    private float SplineDistanceByPosition(Vector3 position)
    {
        var zeroPosition = _moveSpline.GetSample(0f);
        var nearestSplinePoint = _moveSpline.GetProjectionSample(position);

        return Vector3.Distance(zeroPosition.location, nearestSplinePoint.location);
    }

    private Quaternion GetRotationByDirection(Direction direction)
    {
        var sample = _moveSpline.GetSampleAtDistance(_distanceCovered);
        Quaternion rotation;

        if (direction == Direction.Right)
            rotation = sample.Rotation;
        else
            rotation = Quaternion.Euler(-sample.Rotation.eulerAngles.x, -sample.Rotation.eulerAngles.y, sample.Rotation.eulerAngles.z);

        return rotation;
    }

    private void OnDisable()
    {
        _viewZone.Disable();
    }
}
