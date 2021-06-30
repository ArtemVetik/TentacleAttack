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
            var joint = _bones[i].gameObject.AddComponent<HingeJoint>();
            joint.connectedBody = _bones[i - 1].GetComponent<Rigidbody>();
            //joint.useSpring = true;
            //var spring = joint.spring;
            //spring.spring = 100;
            //spring.damper = 10;
            //joint.spring = spring;
        }
    }

    private void FillingBones(Transform parent)
    {
        var collider = parent.gameObject.AddComponent<BoxCollider>();
        parent.gameObject.AddComponent<Rigidbody>();

        //collider.size = new Vector3(0.2f, 0.2f, 0.2f);

        _bones.Add(parent);

        if(parent.childCount > 0)
        {
            FillingBones(parent.GetChild(0));
        }
    }
}