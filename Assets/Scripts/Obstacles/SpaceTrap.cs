using System.Collections;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class SpaceTrap : ActiveObject
{
    [SerializeField] private ParticleSystem _workEffect;
    [SerializeField] private Material _safeMaterial;
    [SerializeField] private GameObject _dangerousObject;

    [SerializeField ]private MeshRenderer _dangerousRenderer;

    public override void Action()
    {
        Material[] materials = new Material[_dangerousRenderer.materials.Length];

        for(int i = 0; i < materials.Length; i++)
        {
            materials[i] = _safeMaterial;
        }

        _dangerousObject.SetActive(false);
        _dangerousRenderer.materials = materials;
        _workEffect.Stop();
    }
} 