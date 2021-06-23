using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TentacleMeshBuilder
{
    private Mesh _mesh;
    private List<Transform> _bones = new List<Transform>();
    private Transform _parentOnScene;
    private SegmentPool _pool;
    private bool _isFirstBuilding = true;

    private readonly Vector3 _defoultRotation = new Vector3(90, 0, 0);
    private const int NUMBER_OF_DECIMAL_PLACES = 2;

    public Mesh GetMesh => _mesh;
    public IEnumerable<Transform> Bones => _bones;


    public TentacleMeshBuilder(Transform parent, SegmentPool pool)
    {
        _parentOnScene = parent;
        _pool = pool;
    }

    public void BuildMesh(params GameObject[] gameObjects)
    {
        _mesh = CombineMeshes(gameObjects);
        float[] yVertixPosition = CollectPositionsY(_mesh).ToArray();
        _mesh.boneWeights = CreateBoneWeight(_mesh, yVertixPosition);

        for (int i = 0; i < yVertixPosition.Length; i++)
        {
            Transform boneParent = i == 0 ? _parentOnScene : _bones[i - 1];
            if (_bones.Count == i)
                AddBone(boneParent, _parentOnScene.position + new Vector3(0, yVertixPosition[i], 0));
            else if (_bones[i] != null)
                ReassignmentBone(i);
        }
    }

    private void AddBone(Transform parent, Vector3 position)
    {
        int boneIndex = _bones.Count;

        _bones.Add(new GameObject("Bone_" + boneIndex).transform);
        _bones[boneIndex].parent = parent;
        _bones[boneIndex].rotation = Quaternion.Euler(_defoultRotation);
        _bones[boneIndex].position = position;

        Matrix4x4[] bindPoses = new Matrix4x4[_mesh.bindposes.Length + 1];
        Array.Copy(_mesh.bindposes, bindPoses, _mesh.bindposes.Length);
        bindPoses[bindPoses.Length - 1] = _bones[boneIndex].worldToLocalMatrix * _parentOnScene.localToWorldMatrix;
        _mesh.bindposes = bindPoses;
    }

    private void ReassignmentBone(int boneIndex)
    {
        Matrix4x4[] bindPoses = new Matrix4x4[_mesh.bindposes.Length + 1];
        Array.Copy(_mesh.bindposes, bindPoses, _mesh.bindposes.Length);
        bindPoses[bindPoses.Length - 1] = _bones[boneIndex].worldToLocalMatrix * _parentOnScene.localToWorldMatrix;
        _mesh.bindposes = bindPoses;
    }

    private void GameObjectsSetActive(bool isActive, params GameObject[] gameObjects)
    {
        foreach (var gameObject in gameObjects)
            gameObject.SetActive(isActive);
    }

    private Mesh CombineMeshes(GameObject[] gameObjects)
    {
        CombineInstance[] combine = new CombineInstance[gameObjects.Length];

        for (int i = 0; i < combine.Length; i++)
        {
            if (gameObjects[i].GetComponent<MeshFilter>() != null)
                combine[i].mesh = gameObjects[i].GetComponent<MeshFilter>().sharedMesh;
            else if(gameObjects[i].GetComponent<SkinnedMeshRenderer>() != null)
                combine[i].mesh = gameObjects[i].GetComponent<SkinnedMeshRenderer>().sharedMesh;

            combine[i].transform = gameObjects[i].transform.localToWorldMatrix;
        }

        if (_isFirstBuilding)
        {
            GameObjectsSetActive(false, gameObjects);
            _isFirstBuilding = false;
        }
        else
            _pool.ReturnSegment();

        Mesh mesh = new Mesh();
        mesh.CombineMeshes(combine);
        mesh.Optimize();

        return mesh;
    }

    private IEnumerable<float> CollectPositionsY(Mesh mesh)
    {
        List<float> yVertixPosition = new List<float>();

        foreach (var vertix in mesh.vertices)
        {
            if (!yVertixPosition.Contains((float)Math.Round(vertix.y, NUMBER_OF_DECIMAL_PLACES)))
            {
                yVertixPosition.Add((float)Math.Round(vertix.y, NUMBER_OF_DECIMAL_PLACES));
            }
        }

        return yVertixPosition.OrderByDescending(y => y);
    }

    private BoneWeight[] CreateBoneWeight(Mesh mesh, float[] groupingValue)
    {
        BoneWeight[] weights = new BoneWeight[mesh.vertexCount];

        for (int i = 0; i < weights.Length; i++)
        {
            for (int j = 0; j < groupingValue.Length; j++)
            {
                if ((float)Math.Round(mesh.vertices[i].y, NUMBER_OF_DECIMAL_PLACES) == groupingValue[j])
                {
                    weights[i].boneIndex0 = j;
                    weights[i].weight0 = 1.0f;
                }
            }
        }

        return weights;
    }

}

