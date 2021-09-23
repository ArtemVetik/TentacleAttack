using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollMultiplyMesh : RagdollEnemy
{
    [SerializeField] private Transform _skinParent;
    [SerializeField] private SkinnedMeshRenderer _skinTemplate;

    public void InitializeMesh(IEnumerable<SkinnedMeshRenderer> skins)
    {
        foreach (var skin in skins)
        {
            var inst = Instantiate(_skinTemplate, _skinParent);
            inst.gameObject.SetActive(true);
            inst.sharedMesh = skin.sharedMesh;
            inst.materials = skin.materials;
        }
    }
}
