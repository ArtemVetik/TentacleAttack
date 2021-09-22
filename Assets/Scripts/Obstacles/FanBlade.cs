using UnityEngine;

public class FanBlade : Obstacle
{
    [SerializeField] private ParticleSystem _hitEffect;

    protected override void Activate()
    {
        if (_hitEffect)
            Instantiate(_hitEffect, transform.position, _hitEffect.transform.rotation);
    }
}
