using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollSingleMesh : RagdollEnemy
{
    [SerializeField] private SkinnedMeshRenderer _skinnedMesh;

    public void InitializeMesh(Mesh mesh)
    {
        _skinnedMesh.sharedMesh = mesh;
    }
}
