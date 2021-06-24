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
    private Vector3 _startPosition;

    public event Action<Vector3> TragetMoved;
    public event Action<Transform> Rewinding;
    public event Action<Transform> RewindFinished;

    private Rigidbody _body;

    private void Awake()
    {
        _body = GetComponent<Rigidbody>();
        _startPosition = transform.position;
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
            Movement();
        if (Input.GetMouseButtonUp(0))
            Rewind();
    }

    private void Movement()
    {
        if (_isRewind)
        {
            _isRewind = false;
            RewindFinished?.Invoke(transform);
        }
        Vector3 translation = _joystick.Direction * _moveSpeed * Time.deltaTime;
        transform.Translate(translation);
        //_body.velocity = translation;

        TragetMoved?.Invoke(transform.position);
    }

    private void Rewind()
    {
        return;

        _isRewind = true;
        Rewinding?.Invoke(transform);
    }
}