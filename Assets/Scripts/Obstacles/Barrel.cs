using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : Obstacle
{
    [SerializeField] private ParticleSystem _explosionEffect;

    protected override void Activate()
    {
        Instantiate(_explosionEffect, transform.position, _explosionEffect.transform.rotation);
        Destroy(gameObject);
    }
}
