using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SplineMesh;

public class SplineCylinderMeshGenerator : MonoBehaviour
{
    [SerializeField] private Spline _spline;

    private float radius = 0.4f;

    private void Start()
    {
        List<Vector3> vertices = new List<Vector3>();
        var tempTransform = new GameObject().transform;

        for (int i = 0; i < _spline.Length; i++)
        {
            var sample = _spline.GetSampleAtDistance(i);

            tempTransform.position = sample.location;

            //var forwardDirection = _spline.GetSampleAtDistance(i + 0.2f).location - _spline.GetSampleAtDistance(i - 0.2f).location;
            tempTransform.rotation = Quaternion.identity;

            for (int angle = 0; angle < 360; angle += 10)
            {
                var position = tempTransform.right * Mathf.Cos(angle * Mathf.Rad2Deg) + tempTransform.up * Mathf.Sin(angle * Mathf.Rad2Deg);

                vertices.Add(position + tempTransform.forward * 0.2f);
                vertices.Add(position - tempTransform.forward * 0.2f);
            }
        }

        var triangles = new int[] { 0, 1, 2, 3, 4, 5 };
        var mesh = GetComponent<MeshFilter>().mesh;

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

    }
}
