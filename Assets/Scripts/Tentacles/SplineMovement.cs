using System;
using System.Collections;
using UnityEngine;
using SplineMesh;

[RequireComponent(typeof(Spline))]
public class SplineMovement : MonoBehaviour
{
    [SerializeField] private float _stepBetweenSplineNodes;
    [SerializeField] private TargetMovement _target;
    [SerializeField] private float _rewindSpeed;

    private Spline _spline;
    private int _lastNodeIndex;
    private bool _isRewind;

    public event Action AddedNode;
    public event Action SplineChanged;

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
        _target.Rewinding += OnTargetRewining;
        _target.RewindFinished += OnTargetRewindFinished;
        GlobalEventStorage.TentacleAddDamageAddListener(OnTentacleAddDamage);
    }

    private void OnDisable()
    {
        _target.TragetMoved -= OnTargetMoved;
        _target.Rewinding -= OnTargetRewining;
        _target.RewindFinished -= OnTargetRewindFinished;
        GlobalEventStorage.TentacleAddDamageRemoveListener(OnTentacleAddDamage);
    }

    private void Start()
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
        _spline.nodes[_lastNodeIndex].Position = position;
        SplineChanged?.Invoke();

        if (_spline.curves[_lastNodeIndex - 1].Length > (_stepBetweenSplineNodes + 1))
            AddNode(position);
        if (IsNeedRemoveNode(1.0f, _stepBetweenSplineNodes / 2))
            RemoveNode();
    }

    private void AddNode(Vector3 position)
    {
        Vector3 lastPosition = GetPositionByDistance(_spline.Length - 1);
        SplineNode node = new SplineNode(position, _spline.nodes[_lastNodeIndex].Direction);
        _spline.nodes[_spline.nodes.Count - 1].Position = lastPosition;
        _spline.AddNode(node);
        _lastNodeIndex++;
    }

    private void RemoveNode()
    {
        Vector3 lastPosition = _spline.nodes[_lastNodeIndex].Position;
        _spline.RemoveNode(_spline.nodes[_lastNodeIndex]);
        _lastNodeIndex--;
        _spline.nodes[_lastNodeIndex].Position = lastPosition;
    }

    private void OnTargetRewining(Transform target)
    {
        _isRewind = true;
        StartCoroutine(RewindSpline(target));
    }

    private void OnTargetRewindFinished(Transform targetTransform)
    {
        _isRewind = false;
        targetTransform.position = _spline.nodes[_lastNodeIndex].Position;
    }

    private IEnumerator RewindSpline(Transform target)
    {

        while (_isRewind)
        {
            Vector3 position = GetPositionByDistance(_spline.Length - _rewindSpeed * Time.deltaTime);
            position.z = 0;
            _spline.nodes[_lastNodeIndex].Position = position;
            SplineChanged?.Invoke();

            if (IsNeedRemoveNode(1f, 0.5f))
                RemoveNode();

            target.position = _spline.nodes[_lastNodeIndex].Position;
            _isRewind = _spline.nodes.Count > 3;

            yield return new WaitForFixedUpdate();
        }
    }

    private bool IsNeedRemoveNode(float minDistance, float minCurveLength)
    {
        float distance = Vector3.Distance(_spline.nodes[_lastNodeIndex].Position, _spline.nodes[_lastNodeIndex - 1].Position);
        return distance < minDistance && _spline.curves[_lastNodeIndex - 1].Length < minCurveLength;
    }

    private void OnTentacleAddDamage()
    {
        gameObject.GetComponent<SplineMeshTiling>().enabled = false;
    }
}