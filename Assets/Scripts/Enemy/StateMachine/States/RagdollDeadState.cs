using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SingleMeshRagdollContainer))]
[RequireComponent(typeof(Enemy))]
public class RagdollDeadState : State
{
    [SerializeField] private ParticleSystem _deadEffect;

    private SingleMeshRagdollContainer _ragdollContainer;
    private Enemy _selfEnemy;

    private void Awake()
    {
        _ragdollContainer = GetComponent<SingleMeshRagdollContainer>();
        _selfEnemy = GetComponent<Enemy>();
    }

    private void OnEnable()
    {
        if (_deadEffect != null)
            Instantiate(_deadEffect, transform.position, _deadEffect.transform.rotation);

        var ragdoll = _ragdollContainer.InstRagdollEnemy(transform.position, transform.rotation);
        ragdoll.EnableRagdoll();

        _selfEnemy.ApplyDamage();

        Destroy(gameObject);
    }
}
