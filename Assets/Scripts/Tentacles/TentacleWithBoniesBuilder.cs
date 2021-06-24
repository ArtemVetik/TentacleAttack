using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TentacleWithBoniesBuilder : MonoBehaviour
{
    [SerializeField] private Transform _segmentsParent;

    private SplineMovement _spline;
    private SkinnedMeshRenderer _meshRenderer;
    private List<Transform> _activeBones;
    private TentacleMeshBuilder _meshBuilder;

    private void OnDisable()
    {
        GlobalEventStorage.TentacleAddDamageRemoveListener(BuildTentacle);
    }

    private void Start()
    {
        _spline = GetComponent<SplineMovement>();

        _meshRenderer = GetComponent<SkinnedMeshRenderer>();
        _activeBones = new List<Transform>();
        _meshBuilder = new TentacleMeshBuilder(transform);
        GlobalEventStorage.TentacleAddDamageAddListener(BuildTentacle);



        //BuildTentacle();
    }

    private void BuildTentacle()
    {
        GameObject[] segments = _segmentsParent.GetComponentsInChildren<TentacleSegment>().Select(segment => segment.gameObject).ToArray();
        _meshBuilder.BuildMesh(segments);

        _meshRenderer.bones = _meshBuilder.Bones.ToArray();
        _meshRenderer.sharedMesh = _meshBuilder.GetMesh;
        _activeBones = _meshBuilder.Bones.ToList();
    }

    private void SetBoniesAlongSpline()
    {
        _activeBones[0].position = _spline.GetPositionByDistance(_spline.SplineLength - 0.02f);
        var forwardVector = _spline.GetPositionByDistance(_spline.SplineLength) - _activeBones[0].position;

        _activeBones[0].rotation = XLookRotation2D(forwardVector);

        for (int i = 1; i < _activeBones.Count; i++)
        {
            Vector3 position = _spline.GetPositionByDistance(_spline.SplineLength - (i));
            _activeBones[i].position = position;
            forwardVector = _spline.GetPositionByDistance(_spline.SplineLength - (i * - 0.02f)) - _activeBones[i].position;
            _activeBones[i].rotation = XLookRotation2D(forwardVector);
        }
    }

    private Quaternion XLookRotation2D(Vector3 forward)
    {
        Quaternion first = Quaternion.Euler(90f, 0f, 0f);
        Quaternion second = Quaternion.LookRotation(Vector3.forward, forward);

        return second * first;
    }


}
