using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.FinalIK;

public class TentacleWithBone : MonoBehaviour
{
    [SerializeField] private Transform _parentBone;
    [SerializeField] private PhysicMaterial _material;
    private List<Transform> _bones;
    private FABRIK _fabrik;

    public int BoneCount => _bones.Count;

    private void Awake()
    {
        _bones = new List<Transform>();
        FillingBones(_parentBone);
        _fabrik = GetComponent<FABRIK>();

        gameObject.SetActive(false);
    }

    public void ShowTentacle(Vector3[] positions)
    {
        gameObject.SetActive(true);
        bool hideUnnecessary = true;

        for (int i = _bones.Count - positions.Length, j = 0; i < _bones.Count && j < positions.Length; i++, j++)
        {
            if(hideUnnecessary)
            {
                foreach (var bone in _bones.Take(_bones.Count - positions.Length))
                    bone.position = positions[0];

                hideUnnecessary = false;
            }

            _bones[i].position = positions[j];
            var collider = _bones[i].gameObject.AddComponent<BoxCollider>();
            var rb = _bones[i].gameObject.AddComponent<Rigidbody>();
            rb.mass = 0.25f;
            rb.drag = 0.1f;
            collider.material = _material;
            _fabrik.solver.AddBone(_bones[i]);

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

