using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class JungleTrap : Obstacle
{
    [SerializeField] private ParticleSystem _attackEffect;

    private Animator _selfAnimator;

    private void Awake()
    {
        _selfAnimator = GetComponent<Animator>();
    }

    protected override void Activate()
    {
        _selfAnimator.SetTrigger("Attack");
        Instantiate(_attackEffect, transform.position + Vector3.up, Quaternion.identity);
    }
}
