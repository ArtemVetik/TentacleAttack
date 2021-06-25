using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.FinalIK;
using SplineMesh;

public class TentacleWithBone : MonoBehaviour
{
    [SerializeField] private Transform _parentBone;
    [SerializeField] private PhysicMaterial _material;
    private List<Transform> _bones;
    //private FABRIK _fabrik;

    public int BoneCount => _bones.Count;

    private void Awake()
    {
        _bones = new List<Transform>();
        FillingBones(_parentBone);
        //_fabrik = GetComponent<FABRIK>();

        gameObject.SetActive(false);
    }

    public void ShowTentacle(Spline spline)
    {
        gameObject.SetActive(true);
        bool hideUnnecessary = true;

        Vector3[] positions = new Vector3[spline.nodes.Count * 4 + 1];

        positions[0] = spline.GetSampleAtDistance(0.01f).location;

        for (int i = 1; i < positions.Length; i++)
        {
            float distance = spline.Length / positions.Length * i;

            if (distance > spline.Length)
                distance = spline.Length - 0.01f;

            positions[i] = spline.GetSampleAtDistance(distance).location;
        }

        for (int i = _bones.Count - positions.Length, j = 0; i < _bones.Count && j < positions.Length; i++, j++)
        {
            if (hideUnnecessary)
            {
                foreach (var bone in _bones.Take(_bones.Count - positions.Length))
                {
                    bone.position = positions[0];
                    //_fabrik.solver.AddBone(bone);
                }

                hideUnnecessary = false;
            }

            _bones[i].position = positions[j];
            var collider = _bones[i].gameObject.AddComponent<BoxCollider>();
            var rb = _bones[i].gameObject.AddComponent<Rigidbody>();

            if (i != _bones.Count - positions.Length)
            {
                var joint = _bones[i - 1].gameObject.AddComponent<ConfigurableJoint>();
                joint.connectedBody = rb;
            }

            //rb.mass = 1f;
            //rb.drag = 2f;
            //rb.angularDrag = 5f;

            rb.constraints = RigidbodyConstraints.FreezePosition;
            //rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;

            //rb.freezeRotation = true;
            collider.material = _material;
            collider.size = new Vector3(0.25f, 0.25f, 0.25f);
            collider.center = new Vector3(0, 0.25f, 0);
            //_fabrik.solver.AddBone(_bones[i]);

        }
    }

    private void FillingBones(Transform parent)
    {
        _bones.Add(parent);
        //parent.gameObject.AddComponent<BoxCollider>();
        //parent.gameObject.AddComponent<Rigidbody>();

        if (parent.childCount > 0)
            FillingBones(parent.GetChild(0));
    }
}

