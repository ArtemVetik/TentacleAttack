using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFolowing : MonoBehaviour
{
    [SerializeField] private Transform _followTarget;

    private void Update()
    {
        var nextPosition = Vector3.Lerp(transform.position, _followTarget.position, 10f * Time.deltaTime);
        nextPosition.z = -15;

        transform.position = nextPosition;
    }
}
