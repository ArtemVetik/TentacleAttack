using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TargetMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private Joystick _joystick;

    public event Action<Vector3> TragetMoved;

    private Rigidbody _body;

    private void Awake()
    {
        _body = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
            Movement();
        if (Input.GetMouseButtonUp(0))
            _body.velocity = Vector3.zero;
    }

    private void Movement()
    {
        Vector3 translation = _joystick.Direction * _moveSpeed * Time.deltaTime;
        //transform.Translate(translation);
        _body.velocity = translation;

        TragetMoved?.Invoke(transform.position);
    }

}