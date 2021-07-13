using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawTrap : Obstacle
{
    [SerializeField] private ParticleSystem _hitEffect;

    protected override void Activate()
    {
        if (_hitEffect)
            Instantiate(_hitEffect, transform.position, _hitEffect.transform.rotation);
    }
}
