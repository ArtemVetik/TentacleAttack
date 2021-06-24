using System;
using UnityEngine;
using SplineMesh;

[RequireComponent(typeof(MeshBender))]
[RequireComponent(typeof(Rigidbody))]
public class TentacleSegment : MonoBehaviour
{
    public Vector3 MeshCenterPosition => _meshBender.SelfMesh.bounds.center;
    public Transform CenterTransform
    {
        get
        {
            _childTransform.position = MeshCenterPosition;
            return _childTransform;
        }      
    }

    private MeshBender _meshBender;
    private Rigidbody _body;
    private Transform _childTransform;

    private void Awake()
    {
        _meshBender = GetComponent<MeshBender>();
        _body = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        var childObject = new GameObject();
        childObject.transform.parent = transform;
        _childTransform = childObject.transform;

        _body.isKinematic = true;
    }

}
