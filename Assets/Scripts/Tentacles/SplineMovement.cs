using System;
using System.Collections;
using UnityEngine;
using SplineMesh;

[RequireComponent(typeof(Spline))]
public class SplineMovement : MonoBehaviour
{
    [SerializeField] private float _stepBetweenSplineNodes;
    [SerializeField] private TargetMovement _target;
    [SerializeField] private float _startRewindSpeed = 10;
    [SerializeField] private float _endRewindSpeed = 30;

    private Spline _spline;
    private int _lastNodeIndex;
    private bool _isRewind;

    public event Action AddedNode;
    public event Action SplineChanged;
    public event Action SplineRewinded;
    public event Action FullRewinded;

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
        GlobalEventStorage.TentacleDiedAddListener(OnTentacleDied);
    }

    private void OnDisable()
    {
        _target.TragetMoved -= OnTargetMoved;
        _target.Rewinding -= OnTargetRewining;
        _target.RewindFinished -= OnTargetRewindFinished;
        GlobalEventStorage.TentacleDiedRemoveListener(OnTentacleDied);
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

        if (_spline.curves[_lastNodeIndex - 1].Length > (_stepBetweenSplineNodes + 1f))
            AddNode(position);
        if (_spline.nodes.Count > 3 && IsNeedRemoveNode(2.0f, _stepBetweenSplineNodes))
            RemoveNode();
    }

    private void AddNode(Vector3 position)
    {
        Vector3 lastPosition = GetPositionByDistance(_spline.Length - 1f);
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

    private void OnTargetRewining(Transform target, float speedRate = 1f)
    {
        _isRewind = true;
        StartCoroutine(RewindSpline(target, speedRate));
    }

    private void OnTargetRewindFinished(Transform targetTransform)
    {
        _isRewind = false;
        targetTransform.position = _spline.nodes[_lastNodeIndex].Position;
    }

    private IEnumerator RewindSpline(Transform target, float speedRate = 1f)
    {
        var currentSpeed = _startRewindSpeed;

        if (_spline.nodes.Count <= 3)
        {
            _isRewind = false;
            FullRewinded?.Invoke();
        }

        while (_isRewind)
        {
            Vector3 position = GetPositionByDistance(_spline.Length - currentSpeed * speedRate * Time.deltaTime);
            position.z = 0;
            _spline.nodes[_lastNodeIndex].Position = position;
            SplineChanged?.Invoke();

            if (IsNeedRemoveNode(1f, 0.5f))
                RemoveNode();

            target.position = _spline.nodes[_lastNodeIndex].Position;

            if (_spline.nodes.Count <= 3)
            {
                _isRewind = false;
                FullRewinded?.Invoke();
            }

            currentSpeed = Mathf.MoveTowards(currentSpeed, _endRewindSpeed, 3f * Time.deltaTime);
            yield return new WaitForFixedUpdate();
        }

        SplineRewinded?.Invoke();
    }

    private bool IsNeedRemoveNode(float minDistance, float minCurveLength)
    {
        float distance = Vector3.Distance(_spline.nodes[_lastNodeIndex].Position, _spline.nodes[_lastNodeIndex - 1].Position);
        return distance < minDistance && _spline.curves[_lastNodeIndex - 1].Length < minCurveLength;
    }

    private void OnTentacleDied()
    {
        gameObject.GetComponent<SplineMeshTiling>().enabled = false;
    }
}