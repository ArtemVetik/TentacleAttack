using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Propane : MonoBehaviour
{
    [SerializeField] private ParticleSystem _smoke;
    [SerializeField] private Transform _smokeContainer;

    private bool _activated = false;

    private void OnTriggerEnter(Collider other)
    {
        if (_activated == false && other.TryGetComponent(out TargetMovement tentacle))
        {
            Instantiate(_smoke, _smokeContainer.position, _smoke.transform.rotation);
            _activated = true;
        }
    }
}
