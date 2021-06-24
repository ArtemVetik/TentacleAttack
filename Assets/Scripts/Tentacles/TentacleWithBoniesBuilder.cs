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


        _tentacle.ShowTentacle(positions);

        //Time.timeScale = 0;
    }

    //private void BuildTentacle()
    //{
    //    GameObject[] segments = _segmentsParent.GetComponentsInChildren<TentacleSegment>().Select(segment => segment.gameObject).ToArray();
    //    //_meshBuilder.BuildMesh(segments);

    //    Vector3[][] verticses = new Vector3[segments.Length][];
    //    Mesh[] meshes = new Mesh[segments.Length];

    //    for(int i = 0; i < segments.Length; i++)
    //    {
    //        meshes[i] = segments[i].GetComponent<MeshFilter>().sharedMesh;
    //        verticses[i] = meshes[i].vertices;
    //    }

    //    CombineInstance[] combine = new CombineInstance[meshes.Length];

    //    for(int i = 0; i < combine.Length; i++)
    //    {
    //        combine[i].mesh = meshes[i];
    //        combine[i].transform = segments[i].transform.localToWorldMatrix;
    //    }

    //    GameObjectsSetActive(false, segments);

    //    Mesh mesh = new Mesh();
    //    mesh.CombineMeshes(combine);
    //    mesh.Optimize();

    //    BoneWeight[] weights = new BoneWeight[mesh.vertexCount];

    //    for(int i = 0; i < verticses.Length; i++)
    //    {
    //        for(int j = 0; j < verticses[i].Length; j++)
    //        {
    //            weights[j].boneIndex0 = i;
    //            weights[i].weight0 = 1;
    //        }
    //    }

    //    mesh.boneWeights = weights;

    //    Matrix4x4[] bindposes = new Matrix4x4[segments.Length];
    //    try
    //        {
    //        for (int i = 0; i < segments.Length; i++)
    //        {
    //            _activeBones.Add(new GameObject("Bone_" + i).transform);
    //            _activeBones[i].parent = i == 0 ? transform : _activeBones[i - 1];
    //            _activeBones[i].rotation = Quaternion.identity;
    //            _activeBones[i].position = _spline.nodes[i].Position;
    //            _activeBones[i].gameObject.AddComponent<BoxCollider>();
    //            _activeBones[i].gameObject.AddComponent<Rigidbody>();

    //            bindposes[i] = _activeBones[i].worldToLocalMatrix * transform.localToWorldMatrix;
    //        }
    //    }
    //    catch(Exception e)
    //    {

    //    }
    //    mesh.bindposes = bindposes;

    //    _meshRenderer.bones = _activeBones.ToArray();
    //    _meshRenderer.sharedMesh = mesh;

    //    Time.timeScale = 0;
    //}


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
