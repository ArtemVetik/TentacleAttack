using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightRagdollContainer : RagdollContainer
{
    [SerializeField] private RagdollKnight _ragdollKnight;
    [SerializeField] private Transform _headContainer;
    [SerializeField] private Transform _bodyContainer;

    private MeshFilter _headMesh;
    private SkinnedMeshRenderer _bodyMesh;

    private void Awake()
    {
        _headMesh = _headContainer.GetComponentInChildren<MeshFilter>();
        _bodyMesh = _bodyContainer.GetComponentInChildren<SkinnedMeshRenderer>();
    }

    public override RagdollEnemy InstRagdollEnemy(Vector3 position, Quaternion rotation)
    {
        var inst = Instantiate(_ragdollKnight, position, rotation);
        Debug.Log(_bodyMesh.sharedMesh.name);
        inst.InitializeMesh(_headMesh.mesh, _bodyMesh.sharedMesh);
        return inst;
    }
}
