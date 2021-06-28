using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ViewZoneDetector : MonoBehaviour
{
    [SerializeField] private ViewZoneMeshGenerator _meshGenerator;
    [SerializeField] private float _maxDistance;
    [SerializeField] private uint _angleDelta = 1;
    [SerializeField] private float _fov;

    public event UnityAction<RaycastHit[]> ObjectsDetected;

    private Transform _lookTransform;

    private void Start()
    {
        _lookTransform = new GameObject("lookTransform").transform;
        _lookTransform.parent = transform;

        _lookTransform.rotation = Quaternion.identity;
    }

    private void FixedUpdate()
    {
        var hitPointList = new List<RaycastHit>((int)(_fov * 2 / _angleDelta));

        var pointList = new List<Vector3>();
        pointList.Add(transform.position);

        for (int angle = -(int)_fov; angle <= _fov; angle += (int)_angleDelta)
        {
            float upDistance = Mathf.Tan(angle * Mathf.Deg2Rad) * _maxDistance;
            _lookTransform.localPosition = Vector3.forward * _maxDistance + Vector3.up * upDistance;
            _lookTransform.LookAt(transform);

            Ray ray = new Ray(transform.position, -1 * _lookTransform.forward);
            var rayDistance = Vector3.Distance(transform.position, _lookTransform.position);

            if (Physics.Raycast(ray, out RaycastHit hitInfo, rayDistance))
            {
                pointList.Add(hitInfo.point);
                hitPointList.Add(hitInfo);
            }
            else
            {
                pointList.Add(_lookTransform.position);
            }
        }

        _meshGenerator.GenerateMesh(pointList);

        if (hitPointList.Count != 0)
            ObjectsDetected?.Invoke(hitPointList.ToArray());
    }

    public void Enable()
    {
        enabled = true;
    }

    public void Disable()
    {
        enabled = false;
        _meshGenerator.ClearMesh();
    }
}