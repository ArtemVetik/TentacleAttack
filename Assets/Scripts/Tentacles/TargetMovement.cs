using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private Transform _target;
    [SerializeField] private Joystick _joystick;

    public event Action<Vector3> TragetMoved;

    private void Update()
    {
        if (Input.GetMouseButton(0))
            Movement();
    }

    private void Movement()
    {
        Vector3 translation = _joystick.Direction * _moveSpeed * Time.deltaTime;
        _target.Translate(translation);

        TragetMoved?.Invoke(_target.position);
    }

}