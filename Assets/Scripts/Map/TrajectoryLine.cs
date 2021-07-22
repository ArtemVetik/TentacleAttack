using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using SplineMesh;

[RequireComponent(typeof(Spline))]
public class TrajectoryLine : MonoBehaviour
{
    [SerializeField] private LineRenderer _line;
    [SerializeField] private StartMovementTrigger _startTrigger;

    private Coroutine _showCoroutine;
    private Spline _trajectory;
    private float _tileDistance = 2f;

    private void Awake()
    {
        _trajectory = GetComponent<Spline>();
    }

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
        _line.positionCount = 0;
        StopCoroutine(_showCoroutine);
        _showCoroutine = null;
    }

    private void Start()
    {
        _showCoroutine = StartCoroutine(ShowLineCoroutine());
    }

    private IEnumerator ShowLineCoroutine()
    {
        while (true)
        {
            _line.positionCount = 0;
            var positions = new List<Vector3>();

            float distance = 0f;
            while (distance < _trajectory.Length)
            {
                positions.Add(_trajectory.GetSampleAtDistance(distance).location);
                _line.positionCount = positions.Count;
                _line.SetPositions(positions.ToArray());

                distance += _tileDistance;
                yield return new WaitForSeconds(0.1f);
            }

            yield return new WaitForSeconds(2f);
        }
    }
}