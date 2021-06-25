using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using SplineMesh;
using System;

public class TentacleWithBone : MonoBehaviour
{
    [SerializeField] private Transform _parentBone;
    [SerializeField] private PhysicMaterial _material;
    private List<Bone> _bones;
    private float _distanceBetweenBonies;

    public int BoneCount => _bones.Count;

    private void Awake()
    {
        _bones = new List<Bone>();
        FillingBones(_parentBone);
        _distanceBetweenBonies = Vector3.Distance(_bones[1].Position, _bones[2].Position);
        gameObject.SetActive(false);
    }

    public void ShowTentacle(Spline spline)
    {
        gameObject.SetActive(true);
        bool hideUnnecessary = true;


        CurveSample[] samples = new CurveSample[(int)(spline.Length / _distanceBetweenBonies) + 1];
        List<Bone> activeBones = new List<Bone>();

        samples[0] = spline.GetSampleAtDistance(0.01f);

        for (int i = 1; i < samples.Length; i++)
        {
            float distance = _distanceBetweenBonies * i;

            if (distance > spline.Length)
                distance = spline.Length - 0.01f;

            samples[i] = spline.GetSampleAtDistance(distance);
        }

        for (int i = _bones.Count - samples.Length, j = 0; i < _bones.Count && j < samples.Length; i++, j++)
        {
            if (hideUnnecessary)
            {
                foreach (var bone in _bones.Take(_bones.Count - samples.Length))
                {
                    bone.SetPosition(samples[0], false);
                }
                hideUnnecessary = false;
            }

            try
            {
                _bones[i].SetPosition(samples[j], true);
                activeBones.Add(_bones[i]);
                _bones[i].FillingBone();
            }
            catch (Exception e)
            {

            }
        }

        for (int i = 0; i < activeBones.Count - 1; i++)
        {
            var joint = activeBones[i].gameObject.AddComponent<HingeJoint>();
            joint.connectedBody = activeBones[i + 1].GetComponent<Rigidbody>();
            joint.useSpring = true;
            var spring = joint.spring;
            spring.spring = 100;
            spring.damper = 10;
            joint.spring = spring;
        }
    }

    private void FillingBones(Transform parent)
    {
        var bone = parent.gameObject.AddComponent<Bone>();
        _bones.Add(bone);

        if (parent.childCount > 0)
            FillingBones(parent.GetChild(0));
    }
}

