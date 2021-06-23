using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TentacleWithSpline : MonoBehaviour
{
    [SerializeField] private float _stepBetweenSegments;
    [SerializeField] private Transform _target;
    [SerializeField] private GameObject _segment;
    [SerializeField] private GameObject[] _startSegments;
    [SerializeField] private SegmentPool _pool;

    private SplineMovement _spline;
    private SkinnedMeshRenderer _meshRenderer;
    private List<Transform> _activeBones;
    private TentacleMeshBuilder _meshBuilder;

    private void OnDisable()
    {
        _spline.AddedNode -= OnAddedNode;
    }

    private void Start()
    {
        _spline = GetComponent<SplineMovement>();
        _spline.AddedNode += OnAddedNode;
        _meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        _activeBones = new List<Transform>();
        _meshBuilder = new TentacleMeshBuilder(transform, _pool);
        _meshBuilder.BuildMesh(_startSegments);
        _target.position = _spline.GetPositionByDistance(_spline.SplineLength);

        _meshRenderer.bones = _meshBuilder.Bones.ToArray(); 
        _meshRenderer.sharedMesh = _meshBuilder.GetMesh;
        _activeBones = _meshBuilder.Bones.ToList();

        SetBoniesAlongSpline();
    }

    private void Update()
    {
        SetBoniesAlongSpline();
    }

    private void SetBoniesAlongSpline()
    {
        _activeBones[0].position = _spline.GetPositionByDistance(_spline.SplineLength - 0.02f);
        var forwardVector = _spline.GetPositionByDistance(_spline.SplineLength) - _activeBones[0].position;

        _activeBones[0].rotation = XLookRotation2D(forwardVector);

        for (int i = 1; i < _activeBones.Count; i++)
        {
            Vector3 position = _spline.GetPositionByDistance(_spline.SplineLength - (i * _stepBetweenSegments));
            _activeBones[i].position = position;
            forwardVector = _spline.GetPositionByDistance(_spline.SplineLength - (i * _stepBetweenSegments - 0.02f)) - _activeBones[i].position;
            _activeBones[i].rotation = XLookRotation2D(forwardVector);
        }
    }

    private Quaternion XLookRotation2D(Vector3 forward)
    {
        Quaternion first = Quaternion.Euler(90f, 0f, 0f);
        Quaternion second = Quaternion.LookRotation(Vector3.forward, forward);

        return second * first;
    }

    private void OnAddedNode()
    {

        _meshBuilder.BuildMesh(_meshRenderer.gameObject, _pool.GetSegment());

        _meshRenderer.bones = _meshBuilder.Bones.ToArray();
        _meshRenderer.sharedMesh = _meshBuilder.GetMesh;
        _activeBones = _meshBuilder.Bones.ToList();

        SetBoniesAlongSpline();
    }

}
