using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TentacleMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private Transform _target;
    [SerializeField] private Joystick _joystick;
    [SerializeField] private TentacleWithSpline _tentacle;

    private void Awake()
    {

    }

    private void Start()
    {

    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
            Movement();
        if(Input.GetKeyDown(KeyCode.Space))
        {
            _tentacle.ShowInformation();
        }
    }

    private void Movement()
    {
        Vector3 translation = _joystick.Direction * _moveSpeed * Time.deltaTime;
        _target.Translate(translation);
        _tentacle.TentacleMove(_target.position);
    }
}