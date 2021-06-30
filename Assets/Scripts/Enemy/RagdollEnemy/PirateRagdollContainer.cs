using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PirateRagdollContainer : RagdollContainer
{
    [SerializeField] private RagdollPirate _ragdollPirate;
    
    private SkinnedMeshRenderer _skinnedMesh;

    private void Awake()
    {
        _skinnedMesh = GetComponentInChildren<SkinnedMeshRenderer>();
    }

    public override RagdollEnemy InstRagdollEnemy(Vector3 position, Quaternion rotation)
    {
        var inst = Instantiate(_ragdollPirate, position, rotation);
        inst.InitializeMesh(_skinnedMesh.sharedMesh);
        return inst;
    }
}
