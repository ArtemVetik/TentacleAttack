using System.Collections;
using UnityEngine;


[ExecuteInEditMode]
public class MeshDebag : MonoBehaviour
{
    [SerializeField] private Transform parent;
    private Mesh _mesh;

    private void OnDrawGizmos()
    {
        if (_mesh == null)
            _mesh = GetComponent<SkinnedMeshRenderer>().sharedMesh;
        if (_mesh != null)
        {
            Debug.Log("Normals - " + _mesh.normals.Length);
            Debug.Log("Verticses - " + _mesh.vertices.Length);
            Debug.Log("Normal 0 - " + _mesh.normals[0]);

            for(int i = 0; i < _mesh.vertices.Length; i++)
            {
                Vector3 position = parent.position + _mesh.vertices[i];
                Gizmos.DrawLine(position, position + _mesh.normals[i]);
            }
        }

    }
}