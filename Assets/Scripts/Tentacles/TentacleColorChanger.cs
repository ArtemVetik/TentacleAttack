using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SplineMesh;

[RequireComponent(typeof(SplineMeshTiling))]
public class TentacleColorChanger : MonoBehaviour
{
    [SerializeField] private Color _startColor;
    [SerializeField] private Color _normalColor;

    private SplineMeshTiling _splineMeshTiling;
    private SplineMovement _splineMovement;
    private Material _tentacleMaterial;

    private void Awake()
    {
        _splineMeshTiling = GetComponent<SplineMeshTiling>();
        _splineMovement = GetComponent<SplineMovement>();
    }

    private void OnEnable()
    {
        _splineMovement.SplineChanged += OnSplineChanged;
    }

    private void OnDisable()
    {
        _splineMovement.SplineChanged -= OnSplineChanged;
    }

    private void Start()
    {
        _tentacleMaterial = _splineMeshTiling.material;

        OnSplineChanged();
    }

    private void OnSplineChanged()
    {
        var parameter = _splineMovement.SplineLength * 0.06f;
        parameter = Mathf.Clamp(parameter, 0, 1);

        _tentacleMaterial.color = Color.LerpUnclamped(_startColor, _normalColor, parameter);
    }
}
