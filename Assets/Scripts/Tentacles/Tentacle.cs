using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using RootMotion.FinalIK;

[RequireComponent(typeof(SkinnedMeshRenderer), typeof(FABRIK))]
public class Tentacle : MonoBehaviour
{
    [SerializeField] private GameObject _cylinder;
    [SerializeField] private int _startSegments;
    
    private SkinnedMeshRenderer _skinnedMesh;
    private FABRIK _fabrik;

    private void Start()
    {
        _skinnedMesh = GetComponent<SkinnedMeshRenderer>();
        _fabrik = GetComponent<FABRIK>();

        GameObject[] cylinders = new GameObject[_startSegments];

        for(int i = 0; i < cylinders.Length; i++)
        {
            cylinders[i] = Instantiate(_cylinder, transform.position + (new Vector3(0, _cylinder.transform.localScale.y * 2, 0) * i), Quaternion.identity);
        }

        CombineInstance[] combine = new CombineInstance[cylinders.Length];

        for (int i = 0; i < cylinders.Length; i++)
        {
            combine[i].mesh = cylinders[i].GetComponent<MeshFilter>().sharedMesh;
            combine[i].transform = cylinders[i].GetComponent<MeshFilter>().transform.localToWorldMatrix;
        }

        DestroyCylinders(cylinders);

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

        bones[0] = new GameObject("Bone_0").transform;
        bones[0].parent = transform;
        bones[0].localRotation = Quaternion.identity;
        bones[0].localPosition = new Vector3(transform.position.x, yVertixPosition[0], transform.position.z);

        bindPoses[0] = bones[0].worldToLocalMatrix * transform.localToWorldMatrix;

        for(int i = 1; i < bones.Length; i++)
        {
            bones[i] = new GameObject("Bone_" + i).transform;
            bones[i].parent = bones[i - 1];
            bones[i].localRotation = Quaternion.identity;
            bones[i].localPosition = new Vector3(0, _cylinder.transform.localScale.y * 2, 0);

            bindPoses[i] = bones[i].worldToLocalMatrix * transform.localToWorldMatrix;
        }
        mesh.bindposes = bindPoses;

        _fabrik.solver.SetChain(bones, bones[0]);
        _skinnedMesh.bones = bones;
        _skinnedMesh.sharedMesh = mesh;

    }

    private void DestroyCylinders(params GameObject[] gameObjects)
    {
        for(int i = 0; i < gameObjects.Length; i++)
        {
            Destroy(gameObjects[i]);
        }
    }

}
