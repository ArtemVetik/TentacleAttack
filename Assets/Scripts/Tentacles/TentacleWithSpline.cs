using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using SplineMesh;

public class TentacleWithSpline : MonoBehaviour
{
    [SerializeField] private float _stepBetweenSplineNodes;
    [SerializeField] private float _stepBetweenSegments;
    [SerializeField] private Transform _target;
    [SerializeField] private GameObject _startSegment;
    [SerializeField] private GameObject _segment;
    [SerializeField] private GameObject[] _startSegments;

    private Spline _spline;
    private SkinnedMeshRenderer _meshRenderer;
    private List<Transform> _activeBones;
    private List<Transform> _staticBones;

    private const int NUMBER_OF_DECIMAL_PLACES = 2;

    private void Start()
    {
        _spline = GetComponent<Spline>();
        _meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        _activeBones = new List<Transform>();
        _staticBones = new List<Transform>();

        for (int i = 0; i < _spline.nodes.Count; i++)
        {
            _spline.nodes[i].Position = new Vector3(0, _stepBetweenSplineNodes * i, 0);
        }

        _target.position = _spline.nodes[_spline.nodes.Count - 1].Position;

        CombineInstance[] combine = new CombineInstance[_startSegments.Length];

        for (int i = 0; i < combine.Length; i++)
        {
            combine[i].mesh = _startSegments[i].GetComponent<MeshFilter>().sharedMesh;
            combine[i].transform = _startSegments[i].transform.localToWorldMatrix;
        }

        HideStartSegments();

        Mesh mesh = new Mesh();
        mesh.CombineMeshes(combine);
        mesh.RecalculateNormals();
        mesh.Optimize();

        List<float> yVertixPosition = new List<float>();

        foreach (var vertix in mesh.vertices)
        {
            if (!yVertixPosition.Contains((float)Math.Round(vertix.y, NUMBER_OF_DECIMAL_PLACES)))
            {
                yVertixPosition.Add((float)Math.Round(vertix.y, NUMBER_OF_DECIMAL_PLACES));
            }
        }

        yVertixPosition = yVertixPosition.OrderByDescending(y => y).ToList();

        BoneWeight[] weights = new BoneWeight[mesh.vertexCount];

        for (int i = 0; i < weights.Length; i++)
        {
            for (int j = 0; j < yVertixPosition.Count; j++)
            {
                if ((float)Math.Round(mesh.vertices[i].y, NUMBER_OF_DECIMAL_PLACES) == yVertixPosition[j])
                {
                    weights[i].boneIndex0 = j;
                    weights[i].weight0 = 1.0f;
                }
            }
        }
        mesh.boneWeights = weights;

        Transform[] bones = new Transform[yVertixPosition.Count];
        Matrix4x4[] bindPoses = new Matrix4x4[yVertixPosition.Count];

        for (int i = 0; i < bones.Length; i++)
        {
            bones[i] = new GameObject("Bone_" + i).transform;
            bones[i].parent = i == 0 ? transform : bones[i - 1];
            bones[i].localRotation = Quaternion.Euler(90, 0, 0);
            bones[i].position = transform.position + new Vector3(0, yVertixPosition[i], 0);

            _activeBones.Add(bones[i]);

            bindPoses[i] = bones[i].worldToLocalMatrix * transform.localToWorldMatrix;
        }
        mesh.bindposes = bindPoses;

        _meshRenderer.bones = bones;
        _meshRenderer.sharedMesh = mesh;

        SetBoniesAlongSpline();
    }

    private void Update()
    {
        //SetBoniesAlongSpline();
    }

    private void HideStartSegments()
    {
        _startSegments[0].SetActive(false);
        _startSegments[1].SetActive(false);
    }

    private Vector3 GetPositionByDistance(float length)
    {
        if (length > 0)
        {
            CurveSample sample = _spline.GetSampleAtDistance(length);
            return sample.location;
        }
        else
        {
            Debug.Log("Distance was zero");
            return Vector3.zero;
        }
    }

    private void SetBoniesAlongSpline()
    {
        _activeBones[0].position = GetPositionByDistance(_spline.Length - 0.2f);
        var forwardVector = GetPositionByDistance(_spline.Length - 0.1f) - _activeBones[0].position;

        LookRotation(_activeBones[0], forwardVector);

        for (int i = 1; i < _activeBones.Count; i++)
        {
            Vector3 position = GetPositionByDistance(_spline.Length - (i * _stepBetweenSegments));
            _activeBones[i].position = position;
            forwardVector = GetPositionByDistance(_spline.Length - (i * _stepBetweenSegments + 0.05f)) - _activeBones[i].position;
            LookRotation(_activeBones[i], forwardVector);
        }
    }

    private void LookRotation(Transform bone, Vector3 forward)
    {
        var direction = forward - bone.position;
        var rotation = Quaternion.LookRotation(direction, Vector3.up);
        rotation.eulerAngles = new Vector3(rotation.eulerAngles.x - 90, rotation.eulerAngles.y, rotation.eulerAngles.z);

        bone.rotation = Quaternion.Lerp(bone.rotation, rotation, 10f * Time.deltaTime);
    }

    public void TentacleMove(Vector3 targetPosition)
    {
        var splineNodes = _spline.nodes;
        splineNodes[splineNodes.Count - 1].Position = targetPosition;
        if (_spline.curves[_spline.curves.Count - 1].Length > _stepBetweenSplineNodes)
        {
            SplineNode node = new SplineNode(targetPosition, splineNodes[splineNodes.Count - 1].Direction);
            _spline.AddNode(node);
        }

        SetBoniesAlongSpline();
    }

    public void ShowInformation()
    {

    }

}
