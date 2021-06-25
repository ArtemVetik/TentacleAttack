using System.Collections;
using UnityEngine;

public class BoneTestRotation : MonoBehaviour
{
    [SerializeField] private Transform _target;


    void Update()
    {
        Vector3 forward = _target.position - transform.position;
        var rotation = Quaternion.LookRotation(forward, Vector3.forward);
        rotation.eulerAngles = new Vector3(rotation.eulerAngles.x - 90f, rotation.eulerAngles.y, rotation.eulerAngles.z);

        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, 10f * Time.deltaTime);
    }
}
