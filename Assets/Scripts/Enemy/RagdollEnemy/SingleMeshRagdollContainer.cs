using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleMeshRagdollContainer : RagdollContainer
{
    [SerializeField] private Transform _skinParent;
    [SerializeField] private RagdollMultiplyMesh _ragdollTemplate;

    public override RagdollEnemy InstRagdollEnemy(Vector3 position, Quaternion rotation)
    {
        var activeSkins = _skinParent.GetComponentsInChildren<SkinnedMeshRenderer>();

        var inst = Instantiate(_ragdollTemplate, position, rotation);
        inst.gameObject.SetActive(true);
        inst.InitializeMesh(activeSkins);
        return inst;
    }
}
