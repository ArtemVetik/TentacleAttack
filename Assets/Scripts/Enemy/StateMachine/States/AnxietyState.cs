using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnxietyState : IdleAnxietyState
{
    [SerializeField] private ViewZoneDetector _zoneDetector;

    private EnemyAnimations _animations;
    private PatrolState _patrolState;

    private void Awake()
    {
        _animations = GetComponentInChildren<EnemyAnimations>();
        _patrolState = GetComponent<PatrolState>();
    }

    protected override void Enable()
    {
        _animations.EnemyAnxiety();

        if (_patrolState.Rotating == false)
            _zoneDetector.Enable();
    }

    private void OnDisable()
    {
        _zoneDetector.Disable();
    }

    private void Update() { }
}
