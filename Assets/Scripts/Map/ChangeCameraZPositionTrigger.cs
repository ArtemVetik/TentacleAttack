using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ChangeCameraZPositionTrigger : MonoBehaviour
{
    [SerializeField] private CameraFolowing _camera;
    [SerializeField] private float _cameraZPosition;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out TargetMovement target))
        {
            _camera.ChangeZPosition(_cameraZPosition);
        }
    }
}
