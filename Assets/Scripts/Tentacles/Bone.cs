using System.Collections;
using UnityEngine;
using SplineMesh;

public class Bone : MonoBehaviour
{
    private Rigidbody _selfRigidbody;
    private BoxCollider _selfCollider;

    private readonly Vector3 _coliderSize = new Vector3(0.2f, 0.2f, 0.2f);

    public Vector3 Position => transform.position;

    public void SetPosition(CurveSample sample, bool isActive)
    {
        transform.position = sample.location;
        transform.rotation = sample.Rotation * Quaternion.Euler(-90,0,0);
    }

    public void FillingBone()
    {
        _selfRigidbody = gameObject.AddComponent<Rigidbody>();
        _selfCollider = gameObject.AddComponent<BoxCollider>();

        _selfRigidbody.mass = 0.01f;
        _selfCollider.size = _coliderSize;
    }
}
