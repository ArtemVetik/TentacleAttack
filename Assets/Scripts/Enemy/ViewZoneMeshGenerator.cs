using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class ViewZoneMeshGenerator : MonoBehaviour
{
    [SerializeField] private Material _renderMaterial;

    private MeshFilter _meshFilter;
    private MeshRenderer _meshRenderer;

    private void Awake()
    {
        _meshFilter = GetComponent<MeshFilter>();
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Start()
    {
        _meshRenderer.material = _renderMaterial;
        _renderMaterial.renderQueue = 3002;
    }

    public void GenerateMesh(List<Vector3> points)
    {
        var mesh = _meshFilter.mesh;

        for (int i = 0; i < points.Count; i++)
            points[i] = transform.InverseTransformPoint(points[i]);

        var triangles = new List<int>();

        for (int i = 1; i < points.Count - 1; i++)
        {
            triangles.Add(0);
            triangles.Add(i);
            triangles.Add(i + 1);
            triangles.Add(0);
            triangles.Add(i + 1);
            triangles.Add(i);
        }

        mesh.vertices = points.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();
    }

    public void ClearMesh()
    {
        _meshFilter.mesh.Clear();
    }
}
