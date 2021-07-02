using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TargetMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private Joystick _joystick;

    private bool _isRewind;
    private bool _isUsed = true;

    public event Action<Vector3> TragetMoved;
    public event Action<Transform, float> Rewinding;
    public event Action<Transform> RewindFinished;

    private Rigidbody _body;
    private EnemyContainer _enemyContainer;

    public bool IsUsed => _isUsed;

    private void Awake()
    {
        _enemyContainer = FindObjectOfType<EnemyContainer>();
        _body = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        GlobalEventStorage.TentacleAddDamageAddListener(ToggleUsed);
        _enemyContainer.EnemyEnded += OnLevelCompleted;
    }

    private void OnDisable()
    {
        GlobalEventStorage.TentacleAddDamageRemoveListener(ToggleUsed);
        _enemyContainer.EnemyEnded -= OnLevelCompleted;
    }

    private void Update()
    {
        if (_isUsed)
        {
            if (Input.GetMouseButton(0))
                Movement();
            if (Input.GetMouseButtonUp(0))
                Rewind();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
            Time.timeScale = Time.timeScale == 0 ? 1 : 0;
    }

    private void Movement()
    {
        if (_isRewind)
        {
            _isRewind = false;
            RewindFinished?.Invoke(transform);
        }

        Vector3 translation = _joystick.Direction * _moveSpeed * Time.deltaTime;
        //var hitColliders = Physics.OverlapSphere(transform.position + translation, 0.25f, 1 << LayerMask.NameToLayer("Map"));

        //if (hitColliders == null || hitColliders.Length == 0)
        {
            //transform.Translate(translation / 50f);
            _body.velocity = translation;
            TragetMoved?.Invoke(transform.position);
        }
    }

    private void Rewind(float speedRate = 1f)
    {
        _body.velocity = Vector3.zero;
        _isRewind = true;
        Rewinding?.Invoke(transform, speedRate);
    }

    private void ToggleUsed()
    {
        _isUsed = !_isUsed;
        var rb = GetComponent<Rigidbody>();
        rb.useGravity = true;
        rb.drag = 0.5f;
    }

    private void OnLevelCompleted()
    {
        _isUsed = false;
        Rewind(2f);
    }
}