using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenGlass : MonoBehaviour
{
    private Rigidbody[] _parts;

    private void Awake()
    {
        _parts = GetComponentsInChildren<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out TargetMovement tentacle))
            EnableGravity();
    }

    private void EnableGravity()
    {
        foreach (var glassPart in _parts)
            glassPart.isKinematic = false;
    }
}
