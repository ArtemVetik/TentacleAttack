using System;
using System.Collections;
using UnityEngine;
using SplineMesh;

[RequireComponent(typeof(Spline))]
[RequireComponent(typeof(SplineMeshTiling))]
public class SplineMovement : MonoBehaviour
{
    [SerializeField] private float _stepBetweenSplineNodes;
    [SerializeField] private TargetMovement _target;
    [SerializeField] private float _startRewindSpeed = 10;
    [SerializeField] private float _endRewindSpeed = 30;

    private Spline _spline;
    private SplineMeshTiling _splineMesh;
    private Coroutine _rewindCoroutine;
    private int _lastNodeIndex;
    private bool _isRewind;
    private float _currentRewindSpeed;
    private float _speedRate;
    private float _accelerationRate;

    public event Action AddedNode;
    public event Action SplineChanged;
    //public event Action<bool> SplineRewinded;
    public event Action<bool> FullRewinded;

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
        GlobalEventStorage.GameOvering += OnTentacleDied;
        GlobalEventStorage.GameEnded += OnGameEnding;
    }

    private void OnDisable()
    {
        _target.TragetMoved -= OnTargetMoved;
        _target.Rewinding -= OnTargetRewining;
        _target.RewindFinished -= OnTargetRewindFinished;
        GlobalEventStorage.GameOvering -= OnTentacleDied;
        GlobalEventStorage.GameEnded -= OnGameEnding;
    }

    private void Start()
    {
        _splineMesh = GetComponent<SplineMeshTiling>();
        _splineMesh.enabled = true;

        _spline = GetComponent<Spline>();
        _lastNodeIndex = _spline.nodes.Count - 1;
        _target.transform.position = _spline.nodes[_lastNodeIndex].Position;
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

    private void OnTargetRewining(Transform target, float speedRate = 1f, float accelerationRate = 1f)
    {
        _currentRewindSpeed = _startRewindSpeed;
        _speedRate = speedRate;
        _accelerationRate = accelerationRate;

        if (_spline.nodes.Count <= 3)
        {
            FullRewinded?.Invoke(true);
            //SplineRewinded?.Invoke(true);
            return;
        }

        _isRewind = true;
    }

    private void OnTargetRewindFinished(Transform targetTransform)
    {
        _isRewind = false;
        targetTransform.position = _spline.nodes[_lastNodeIndex].Position;
        //SplineRewinded?.Invoke(true);
    }

    private void Update()
    {
        Vector3 position;
        if (_isRewind)
        {
            var rewindDistance = _currentRewindSpeed * Time.deltaTime;
            position = GetPositionByDistance(_spline.Length - rewindDistance);
            position.z = 0;

            while (rewindDistance > _spline.curves[_lastNodeIndex - 1].Length && _spline.nodes.Count > 3)
            {
                rewindDistance -= _spline.curves[_lastNodeIndex - 1].Length;
                RemoveNode();
            }

            _spline.nodes[_lastNodeIndex].Position = position;
            _target.transform.position = _spline.nodes[_lastNodeIndex].Position;

            SplineChanged?.Invoke();

            if (_spline.nodes.Count <= 3)
            {
                _isRewind = false;
                FullRewinded?.Invoke(true);
            }

            if (IsNeedRemoveNode(1f, 0.5f))
                RemoveNode();

            _currentRewindSpeed = Mathf.MoveTowards(_currentRewindSpeed, _endRewindSpeed * _speedRate, 3f * _accelerationRate * Time.deltaTime);
        }
    }

    private IEnumerator RewindSpline(Transform target, float speedRate = 1f, float accelerationRate = 1f)
    {
        var currentSpeed = _startRewindSpeed;

        if (_spline.nodes.Count <= 3)
        {
            _isRewind = false;
            FullRewinded?.Invoke(true);
        }

        Vector3 position;
        while (_isRewind)
        {
            position = GetPositionByDistance(_spline.Length - currentSpeed * Time.deltaTime);
            position.z = 0;
            _spline.nodes[_lastNodeIndex].Position = position;
            SplineChanged?.Invoke();

            if (IsNeedRemoveNode(1f, 0.5f))
                RemoveNode();

            target.position = _spline.nodes[_lastNodeIndex].Position;

            if (_spline.nodes.Count <= 3)
            {
                _isRewind = false;
                FullRewinded?.Invoke(true);
            }

            currentSpeed = Mathf.MoveTowards(currentSpeed, _endRewindSpeed * speedRate, 3f * accelerationRate * Time.deltaTime);
            yield return null;
        }

        //SplineRewinded?.Invoke(true);
    }

    private bool IsNeedRemoveNode(float minDistance, float minCurveLength)
    {
        float distance = Vector3.Distance(_spline.nodes[_lastNodeIndex].Position, _spline.nodes[_lastNodeIndex - 1].Position);
        return distance < minDistance && _spline.curves[_lastNodeIndex - 1].Length < minCurveLength;
    }

    private void OnTentacleDied(bool isWin)
    {
        if (!isWin)
        {
            gameObject.GetComponent<SplineMeshTiling>().enabled = false;
        }
    }

    private void OnGameEnding(bool isWin, int progress)
    {
        gameObject.SetActive(!isWin);
    }
}