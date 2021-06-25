using System.Collections;
using UnityEngine;

public class Bone : MonoBehaviour
{
    private Rigidbody _selfRigidbody;
    private BoxCollider _selfCollider;

    private readonly Vector3 _coliderSize = new Vector3(0.2f, 0.2f, 0.2f);

    public Vector3 Position => transform.position;

    public void SetPosition(Vector3 position, bool isActive)
    {
        transform.position = position;
    }

    public void FillingBone()
    {
        _selfRigidbody = gameObject.AddComponent<Rigidbody>();
        _selfCollider = gameObject.AddComponent<BoxCollider>();

        _selfRigidbody.mass = 1.0f;
        _selfCollider.size = _coliderSize;
    }
}
