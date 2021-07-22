using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleAnxietyState : State
{
    [SerializeField] private ParticleSystem _anxietyEffect;

    private void OnEnable()
    {
        Instantiate(_anxietyEffect, transform.position - Vector3.forward + Vector3.up * 2f, Quaternion.identity);

        Enable();
    }

    protected virtual void Enable() { }

    private void Update() { }
}
