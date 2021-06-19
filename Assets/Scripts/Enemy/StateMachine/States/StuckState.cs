using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StuckState : State
{
    private EnemyAnimations _animations;
    private Quaternion _forwardRotation;

    private void Awake()
    {
        _animations = GetComponentInChildren<EnemyAnimations>();
    }

    private void OnEnable()
    {
        _animations.EnemyStuck();
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
