using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ViewZoneDetector : MonoBehaviour
{
    [SerializeField] private ViewZoneMeshGenerator _meshGenerator;
    [SerializeField] private float _maxDistance;
    [SerializeField] private uint _angleDelta = 2;
    [SerializeField] private float _fov;

    public event UnityAction<RaycastHit[]> ObjectsDetected;

    private Transform _lookTransform;
    private List<RaycastHit> _hitPointList;
    private List<Vector3> _pointList;

    private void Start()
    {
        _lookTransform = new GameObject("lookTransform").transform;
        _lookTransform.parent = transform;

        _lookTransform.rotation = Quaternion.identity;

        _hitPointList = new List<RaycastHit>((int)(_fov * 2 / _angleDelta));
        _pointList = new List<Vector3>();
    }

    private void LateUpdate()
    {
        _hitPointList.Clear();
        _pointList.Clear();

        _pointList.Add(transform.position);

        Ray ray;
        for (int angle = -(int)_fov; angle <= _fov; angle += (int)_angleDelta)
        {
            var upDistance = Mathf.Tan(angle * Mathf.Deg2Rad) * _maxDistance;
            _lookTransform.localPosition = Vector3.forward * _maxDistance + Vector3.up * upDistance;
            _lookTransform.LookAt(transform);

            ray = new Ray(transform.position, -1 * _lookTransform.forward);
            var rayDistance = Vector3.Distance(transform.position, _lookTransform.position);

            int cubeLayerIndex = LayerMask.NameToLayer("EnemyIgnored");
            int layerMask = (1 << cubeLayerIndex);
            layerMask = ~layerMask;

            if (Physics.Raycast(ray, out RaycastHit hitInfo, rayDistance, layerMask))
            {
                _pointList.Add(hitInfo.point);
                _hitPointList.Add(hitInfo);
            }
            else
            {
                _pointList.Add(_lookTransform.position);
            }
        }

        _meshGenerator.GenerateMesh(_pointList);

        if (_hitPointList.Count != 0)
            ObjectsDetected?.Invoke(_hitPointList.ToArray());
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