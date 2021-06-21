using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StuckState : State
{
    [SerializeField] private ParticleSystem _stuckEffect;

    private EnemyAnimations _animations;
    private Quaternion _forwardRotation;

    private void Awake()
    {
        _animations = GetComponentInChildren<EnemyAnimations>();
    }

    private void OnEnable()
    {
        _animations.EnemyStuck();
        Instantiate(_stuckEffect, transform.position - Vector3.forward + Vector3.up * 2f, Quaternion.identity);
    }

    private void Start()
    {
        _forwardRotation = Quaternion.Euler(0,180,0);
    }

    private void Update()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, _forwardRotation, Time.deltaTime);
    }
}
