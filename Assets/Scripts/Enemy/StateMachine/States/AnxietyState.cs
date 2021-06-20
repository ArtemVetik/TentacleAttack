using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnxietyState : IdleAnxietyState
{
    [SerializeField] private EnemyAnimations _animations;
    [SerializeField] private ViewZoneDetector _zoneDetector;

    protected override void Enable()
    {
        _animations.EnemyAnxiety();
        _zoneDetector.Enable();
    }

    private void OnDisable()
    {
        _zoneDetector.Disable();
    }

    private void Update() { }
}
