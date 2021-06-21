using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayTransition : Transition
{
    [SerializeField] private float _delay;

    private float _accumulatedTime;

    protected override void Enable()
    {
        _accumulatedTime = 0f;
    }

    private void Update()
    {
        _accumulatedTime += Time.deltaTime;

        if (_accumulatedTime > _delay)
            NeedTransit = true;
    }
}
