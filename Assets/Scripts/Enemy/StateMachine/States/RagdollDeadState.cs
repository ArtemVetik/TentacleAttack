using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SingleMeshRagdollContainer))]
public class RagdollDeadState : State
{
    [SerializeField] private ParticleSystem _deadEffect;

    private SingleMeshRagdollContainer _ragdollContainer;

    private void Awake()
    {
        _ragdollContainer = GetComponent<SingleMeshRagdollContainer>();
    }

    private void OnEnable()
    {
        if (_deadEffect != null)
            Instantiate(_deadEffect, transform.position, _deadEffect.transform.rotation);

        var ragdoll = _ragdollContainer.InstRagdollEnemy(transform.position, transform.rotation);
        ragdoll.EnableRagdoll();

        Destroy(gameObject);
    }
}
