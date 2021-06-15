using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TentacleSkeleton : MonoBehaviour
{
    [SerializeField] private Transform _skeletonRoot;

    private List<Transform> _allBones;

    public Transform BonesRoot => _skeletonRoot;
    public List<Transform> AllBones => _allBones;

    private void Awake()
    {
        InitSkeleton();
    }

    private void InitSkeleton()
    {
        _allBones = new List<Transform>();

        var parent = _skeletonRoot;
        while (parent.childCount > 0)
        {
            _allBones.Add(parent.GetChild(0);
            parent = parent.GetChild(0);
        }
    }
}
