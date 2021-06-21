using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using RootMotion.FinalIK;

[RequireComponent(typeof(SkinnedMeshRenderer), typeof(FABRIK))]
public class Tentacle : MonoBehaviour
{
    [SerializeField] private GameObject _sigment;
    [SerializeField] private int _startSegmentCount;
    [SerializeField] private GameObject _startSegment;
    [SerializeField] private Transform _target;

    private SkinnedMeshRenderer _skinnedMesh;
    private FABRIK _fabrik;
    private int _segmentCount;
    private List<Transform> _bones;
    private Track _track;
    private const float SEGMENT_STEP = 1.0f;

    private void Start()
    {
        _skinnedMesh = GetComponent<SkinnedMeshRenderer>();
        _fabrik = GetComponent<FABRIK>();
        _segmentCount = 1;
        _bones = new List<Transform>();
        _track = new Track();

        GameObject[] segments = new GameObject[_startSegmentCount];

        Vector3 position = transform.position + Vector3.up * SEGMENT_STEP * _startSegmentCount;
        _target.position = position;
        segments[0] = Instantiate(_startSegment, position, Quaternion.identity);

        position = segments[0].transform.position + Vector3.down * (SEGMENT_STEP * 2);

        for (int i = 1; i < segments.Length; i++)
        {
            segments[i] = Instantiate(_sigment, position, Quaternion.identity);
            position = segments[i].transform.position + Vector3.down * SEGMENT_STEP;
        }

        CombineInstance[] combine = new CombineInstance[segments.Length];

        for (int i = 0; i < segments.Length; i++)
        {
            combine[i].mesh = segments[i].GetComponent<MeshFilter>().sharedMesh;
            combine[i].transform = segments[i].transform.localToWorldMatrix;
        }

        DestroyCylinders(segments);

        Mesh mesh = new Mesh();
        mesh.CombineMeshes(combine);
        mesh.Optimize();

        List<float> yVertixPosition = new List<float>();

        foreach (var vertix in mesh.vertices)
        {
            if (!yVertixPosition.Contains(vertix.y))
            {
                yVertixPosition.Add(vertix.y);
            }
        }

        yVertixPosition = yVertixPosition.OrderBy(y => y).ToList();

        BoneWeight[] weights = new BoneWeight[mesh.vertexCount];

        for (int i = 0; i < weights.Length; i++)
        {
            for (int j = 0; j < yVertixPosition.Count; j++)
            {
                if (mesh.vertices[i].y == yVertixPosition[j])
                {
                    weights[i].boneIndex0 = j;
                    weights[i].weight0 = 1;
                }
            }
        }

        mesh.boneWeights = weights;

        Transform[] bones = new Transform[yVertixPosition.Count];
        Matrix4x4[] bindPoses = new Matrix4x4[yVertixPosition.Count];

        for (int i = 0; i < bones.Length; i++)
        {
            bones[i] = new GameObject("Bone_" + i).transform;
            bones[i].parent = i == 0? transform : bones[i - 1];
            bones[i].localRotation = Quaternion.identity;
            bones[i].localPosition = new Vector3(transform.position.x, i == 0 ? yVertixPosition[0] : SEGMENT_STEP * 1, transform.position.z);
            _bones.Add(bones[i]);
            _track.AddPoint(bones[i].position);

            bindPoses[i] = bones[i].worldToLocalMatrix * transform.localToWorldMatrix;
        }
        mesh.bindposes = bindPoses;

        _fabrik.solver.SetChain(bones, bones[0]);
        _skinnedMesh.bones = bones;
        _skinnedMesh.sharedMesh = mesh;
    }

    private void DestroyCylinders(params GameObject[] gameObjects)
    {
        for (int i = 0; i < gameObjects.Length; i++)
        {
            Destroy(gameObjects[i]);
        }
    }

    public void TentacleMove(float speed)
    {
        _track.AddPoint(_target.position);
        _bones[0].position = _track.GetTrackPosition(_bones[0].position, speed * Time.deltaTime);

    }

    public void ShowInformation()
    {
        foreach (var bone in _fabrik.solver.bones)
        {
            Debug.Log("Solver position - " + bone.solverPosition);
        }
    }
}
