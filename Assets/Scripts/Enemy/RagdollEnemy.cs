using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.FinalIK;

public class RagdollEnemy : MonoBehaviour
{
    [SerializeField] private RagdollUtility _ragdollUtility;

    private SkinnedMeshRenderer _meshRenderer;

    private void Awake()
    {
        _meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
    }

    public void SetMesh(Mesh mesh)
    {
        _meshRenderer.sharedMesh = mesh;
    }

    public void EnableRagdoll()
    {
        _ragdollUtility.EnableRagdoll();
    }
}
