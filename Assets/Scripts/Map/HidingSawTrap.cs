using UnityEngine;
using System;
using SplineMesh;
using System.Collections;

public class HidingSawTrap : ActiveObject
{
    public enum HideSide
    {
        HideX, HideY, HideZ, HideXRev, HideYRev, HideZRev,
    }

    [SerializeField] private HideSide _hideSide;
    [SerializeField] private bool _isMoving;
    [SerializeField] private float _speed;

    private Animator _selfAnimator;
    private Spline _spline;
    private TargetMovement _tentacle;
    private SphereCollider _rootCollider;
    private Coroutine _moving;
    private float _distanceCovered;
    private Direction _direction;

    private void Awake()
    {
        _selfAnimator = GetComponent<Animator>();
        _spline = GetComponentInParent<Spline>();
        _rootCollider = GetComponentInChildren<SphereCollider>();
        _tentacle = FindObjectOfType<TargetMovement>();
    }

    private void OnEnable()
    {
        _tentacle.TragetMoved += OnTargetMoved;
    }

    private void OnDisable()
    {
        _tentacle.TragetMoved -= OnTargetMoved;
    }

    private void OnTargetMoved(Vector3 position)
    {
        if (_isMoving == false || _rootCollider == null || _spline == null)
            return;

        var splinePos = _spline.GetProjectionSample(position).location;
        var distance = Vector3.Distance(splinePos, position);
        
        if (distance <= _rootCollider.radius * 1.2f)
        {
            if (_moving != null)
            {
                StopCoroutine(_moving);
                _moving = null;
            }
        }

    }

    private void Start()
    {
        if (_isMoving && _spline != null)
        {
            _distanceCovered = 0f;
            _direction = Direction.Right;
            transform.position = _spline.GetSampleAtDistance(_distanceCovered).location;

            _moving = StartCoroutine(Moving());
        }
    }

    public override void Action()
    {
        string stringValue = Enum.GetName(typeof(HideSide), _hideSide);
        if (_moving != null)
        {
            StopCoroutine(_moving);
            _moving = null;
        }

        _selfAnimator.SetTrigger(stringValue);
    }

    private IEnumerator Moving()
    {
        while (true)
        {
            _distanceCovered += _speed * Time.deltaTime * (int)_direction;

            if (_distanceCovered >= _spline.Length || _distanceCovered < 0)
            {
                _direction = _direction == Direction.Left ? Direction.Right : Direction.Left;
                _distanceCovered = _distanceCovered < 0 ? 0 : _spline.Length - 0.01f;
            }

            transform.position = _spline.GetSampleAtDistance(_distanceCovered).location;

            yield return null;
        }
    }
}
