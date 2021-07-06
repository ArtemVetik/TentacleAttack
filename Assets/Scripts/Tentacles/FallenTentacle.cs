using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallenTentacle : MonoBehaviour
{
    [SerializeField] private Transform _parent;

    private List<Transform> _bones = new List<Transform>();

    void Start()
    {
        FillingBones(_parent);

        for(int i = 1; i < _bones.Count; i++)
        {
            var joint = _bones[i].gameObject.AddComponent<CharacterJoint>();
            joint.connectedBody = _bones[i - 1].GetComponent<Rigidbody>();
        }
    }

    private void FillingBones(Transform parent)
    {
        parent.gameObject.AddComponent<BoxCollider>();
        parent.gameObject.AddComponent<Rigidbody>();

        _bones.Add(parent);

        if(parent.childCount > 0)
        {
            FillingBones(parent.GetChild(0));
        }
    }
}