using System.Collections;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class SpaceTrap : ActiveObject
{
    [SerializeField] private ParticleSystem _workEffect;
    [SerializeField] private Material _safeMaterial;

    private MeshRenderer _selfRenderer;

    private void Start()
    {
        _selfRenderer = GetComponent<MeshRenderer>();
    }

    public override void Action()
    {
        
    }
}