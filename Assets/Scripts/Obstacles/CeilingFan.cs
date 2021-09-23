using System.Collections;
using UnityEngine;

public class CeilingFan : ActiveObject
{
    [SerializeField] private ParticleSystem _disableEffect;
    [SerializeField] private GameObject[] _disablableObjects;
    private Animator _selfAnimator;
    private MeshRenderer _selfMeshRenderer;

    private void Start()
    {
        _selfAnimator = GetComponent<Animator>();
        _selfMeshRenderer = GetComponent<MeshRenderer>();
    }

    public override void Action()
    {
        _selfAnimator.enabled = false;

        if (_selfMeshRenderer != null)
            _selfMeshRenderer.enabled = false;

        foreach (var disabeleObject in _disablableObjects)
        {
            Instantiate(_disableEffect, disabeleObject.transform.position, Quaternion.identity);
            disabeleObject.SetActive(false);
        }
    }
}