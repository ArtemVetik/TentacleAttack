using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectObstacle : Obstacle
{
    [SerializeField] private ParticleSystem _activateEffect;

    protected override void Activate()
    {
        Instantiate(_activateEffect, transform.position, Quaternion.identity);
    }
}
