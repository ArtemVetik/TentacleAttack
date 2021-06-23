using System;
using System.Collections;
using UnityEngine;
using SplineMesh;

[RequireComponent(typeof(Spline))]
public class SplineMovement : MonoBehaviour
{
    [SerializeField] private float _stepBetweenSplineNodes;
    [SerializeField] private TargetMovement _target;

    private Spline _spline;
    private int _lastNodeIndex;

    public event Action AddedNode;

    public float SplineLength
    {
        get
        {
            if (_spline == null)
                _spline = GetComponent<Spline>();
            return _spline.Length;
        }
    }

    private void OnEnable()
    {
        _target.TragetMoved += OnTargetMoved;
    }

    private void OnDisable()
    {
        _target.TragetMoved -= OnTargetMoved;
    }

    void Start()
    {
        _spline = GetComponent<Spline>();
        _lastNodeIndex = _spline.nodes.Count - 1;
    }

    public Vector3 GetPositionByDistance(float length)
    {
        if (length > 0)
        {
            CurveSample sample = _spline.GetSampleAtDistance(length);
            return sample.location;
        }
        else
        {
            Debug.Log("Distance was zero");
            return Vector3.zero;
        }
    }

    private void OnTargetMoved(Vector3 position)
    {
        var splineNodes = _spline.nodes;
        splineNodes[_lastNodeIndex].Position = position;
        if (_spline.curves[_lastNodeIndex - 1].Length > (_stepBetweenSplineNodes + 1))
        {
            Vector3 lastPosition = GetPositionByDistance(_spline.Length - 1);
            SplineNode node = new SplineNode(position, splineNodes[_lastNodeIndex].Direction);
            _spline.nodes[_spline.nodes.Count - 1].Position = lastPosition;
            _spline.AddNode(node);
            _lastNodeIndex++;
            //AddedNode?.Invoke();
        }
    }
}