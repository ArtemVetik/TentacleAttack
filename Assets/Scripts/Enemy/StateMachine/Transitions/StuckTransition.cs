using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StuckTransition : Transition
{
    private Enemy _selfEnemy;

    private void Awake()
    {
        _selfEnemy = GetComponentInParent<Enemy>();
    }

    protected override void Enable()
    {
        _selfEnemy.Stucked += OnStucked;
    }

    private void OnDisable()
    {
        _selfEnemy.Stucked -= OnStucked;
    }

    private void OnStucked(Enemy enemy)
    {
        NeedTransit = true;
    }

    private void Update() { }
}
