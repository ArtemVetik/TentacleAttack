using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnxietyState : State
{
    [SerializeField] private EnemyAnimations _animations;
    [SerializeField] private ViewZoneDetector _zoneDetector;
    [SerializeField] private ParticleSystem _anxietyEffect;

    private void OnEnable()
    {
        _animations.EnemyAnxiety();
        Instantiate(_anxietyEffect, transform);

        _zoneDetector.Enable();
    }

    private void OnDisable()
    {
        _zoneDetector.Disable();
    }

    private void Update() { }
}
