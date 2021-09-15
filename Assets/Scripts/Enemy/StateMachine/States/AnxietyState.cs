using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnxietyState : IdleAnxietyState
{
    private EnemyAnimations _animations;

    private void Awake()
    {
        _animations = GetComponentInChildren<EnemyAnimations>();
    }

    protected override void Enable()
    {
        _animations.EnemyAnxiety();
    }

    private void Update() { }
}
