using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using SplineMesh;

public class TentacleWithBoniesBuilder : MonoBehaviour
{
    [SerializeField] private Transform _segmentsParent;
    [SerializeField] private TentacleWithBone _tentacle;

    private Spline _spline;
    private SkinnedMeshRenderer _meshRenderer;
    private List<Transform> _activeBones;
    private TentacleMeshBuilder _meshBuilder;

    private void OnDisable()
    {
        GlobalEventStorage.TentacleAddDamageRemoveListener(BuildTentacle);
    }

    private void Start()
    {
        _spline = GetComponentInParent<Spline>();

        _meshRenderer = GetComponent<SkinnedMeshRenderer>();
        _activeBones = new List<Transform>();
        _meshBuilder = new TentacleMeshBuilder(transform);
        GlobalEventStorage.TentacleAddDamageAddListener(BuildTentacle);
    }

    private void BuildTentacle()
    {
        GameObject[] segments = _segmentsParent.GetComponentsInChildren<TentacleSegment>().Select(segment => segment.gameObject).ToArray();
        var positions = _spline.nodes.Select(node => node.Position).ToArray();

        if (segments.Length <= _tentacle.BoneCount)
            GameObjectsSetActive(false, segments);
        else if(segments.Length > _tentacle.BoneCount)
        {
            var orderSegments = segments.Reverse().ToArray();

            GameObjectsSetActive(false, orderSegments.Take(_tentacle.BoneCount).ToArray());
        }


        _tentacle.ShowTentacle(_spline);
    }

     private Quaternion XLookRotation2D(Vector3 forward)
    {
        Quaternion first = Quaternion.Euler(90f, 0f, 0f);
        Quaternion second = Quaternion.LookRotation(Vector3.forward, forward);

        return second * first;
    }

    private void GameObjectsSetActive(bool isActive, params GameObject[] gameObjects)
    {
        foreach (var gameObject in gameObjects)
            gameObject.SetActive(isActive);
    }
}
