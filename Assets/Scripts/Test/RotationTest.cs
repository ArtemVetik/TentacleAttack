using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationTest : MonoBehaviour
{
    public Transform target;
    public float speed = 1.0f;

    private Quaternion _lookRotation;
    private Vector3 _direction;

    void Update()
    {
        _direction = target.position - transform.position;
        var rotation = Quaternion.LookRotation(_direction, Vector3.back);
        var trRotation = transform.rotation;
        trRotation.z = rotation.x;
        transform.rotation = trRotation;


    }

    private void RotateWithTransformRotateAround()
    {
        _direction = target.position - transform.position;

        float angle = Vector3.SignedAngle(_direction, transform.up, Vector3.forward);

        Debug.Log(angle);
        Vector3 rotation = transform.rotation.eulerAngles;
        transform.RotateAround(transform.position, Vector3.back, angle * (Mathf.PI / 180));
    }
}
