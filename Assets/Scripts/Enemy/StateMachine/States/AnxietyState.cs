using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnxietyState : IdleAnxietyState
{
    [SerializeField] private ViewZoneDetector _zoneDetector;

    private EnemyAnimations _animations;
    
    private void Awake()
    {
        _animations = GetComponentInChildren<EnemyAnimations>();
    }

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
