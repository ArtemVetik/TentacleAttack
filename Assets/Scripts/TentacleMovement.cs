using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TentacleMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;

    private TentacleSkeleton _skeleton;
    private List<Transform> _rotatePoints;

    private void Awake()
    {
        _skeleton = GetComponentInChildren<TentacleSkeleton>();
    }

    private void Update()
    {
        
    }
}