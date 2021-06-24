using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using UnityEditor;

public class TestVertix : MonoBehaviour
{
    private Mesh _mesh;

    private void OnValidate()
    {
        _mesh = GetComponent<MeshFilter>().sharedMesh;
        //_mesh = GetComponent<SkinnedMeshRenderer>().sharedMesh;
    }

    private void Update()
    {
        if(_mesh == null)
            _mesh = GetComponent<SkinnedMeshRenderer>().sharedMesh;
    }

    private void OnDrawGizmos()
    {
        if(_mesh != null)
        {
            List<float> yPosition = new List<float>();

            foreach (var vertix in _mesh.vertices)
            {
                if (!yPosition.Contains((float)Math.Round(vertix.y, 2)))
                {
                    yPosition.Add((float)Math.Round(vertix.y, 2));
                }
            }

            yPosition = yPosition.OrderBy(y => y).ToList();

            Vector3 position = transform.position;

            var vertixWithOnceYPOsition = _mesh.vertices.Where(vertix => (float)Math.Round(vertix.y, 2) == yPosition[3]).ToArray();

            Gizmos.color = Color.cyan;

            foreach(var vertix in vertixWithOnceYPOsition)
            {
                Gizmos.DrawSphere(position + vertix, 0.1f);
            }

            Debug.Log("Lenght - " + vertixWithOnceYPOsition.Length);

            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(position + new Vector3(0, yPosition[0], 0), 0.1f);

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(position +  new Vector3(0, yPosition[yPosition.Count - 1], 0), 0.1f);

            Debug.Log("firstPos - " + new Vector3(position.x, yPosition[0], position.z));
            Debug.Log("secondPos - " + new Vector3(position.x, yPosition[yPosition.Count - 1], position.z));
            Debug.Log("y count - " + yPosition.Count);
        }
            
    }

}
