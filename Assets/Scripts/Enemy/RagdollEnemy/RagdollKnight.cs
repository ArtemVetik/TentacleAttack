using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollKnight : RagdollEnemy
{
    [SerializeField] private MeshFilter _head;
    [SerializeField] private SkinnedMeshRenderer _body;

    public void InitializeMesh(Mesh head, Mesh body)
    {
        _head.mesh = head;
        _body.sharedMesh = body;
    }
}
