using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleMeshRagdollContainer : RagdollContainer
{
    [SerializeField] private RagdollSingleMesh _ragdollTemplate;
    
    private SkinnedMeshRenderer _skinnedMesh;

    private void Awake()
    {
        _skinnedMesh = GetComponentInChildren<SkinnedMeshRenderer>();
    }

    public override RagdollEnemy InstRagdollEnemy(Vector3 position, Quaternion rotation)
    {
        var inst = Instantiate(_ragdollTemplate, position, rotation);
        inst.InitializeMesh(_skinnedMesh.sharedMesh);
        return inst;
    }
}
