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

    private Spline _moveSpline;
    private float _distanceCovered;
    private Direction _directon;

    private void Awake()
    {
        _moveSpline = GetComponentInParent<Spline>();
    }

    private void OnEnable()
    {
        _viewZone.Enable();
        _distanceCovered = SplineDistanceByPosition(transform.position);
        _directon = transform.eulerAngles.y > 180 ? Direction.Right : Direction.Left;
    }

    private void Update()
    {
        _distanceCovered += _speed * Time.deltaTime * (int)_directon;

        if (_distanceCovered > _moveSpline.Length)
        {
            _distanceCovered = _moveSpline.Length;
            _directon = Direction.Left;
        }
        if (_distanceCovered < 0)
        {
            _distanceCovered = 0;
            _directon = Direction.Right;
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

    private void OnDisable()
    {
        _viewZone.Disable();
    }
}
