using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TentacleDetectedTransition : Transition
{
    [SerializeField] private ViewZoneDetector _detector;
    [SerializeField] private PlayerContainer _container;

    protected override void Enable()
    {
        _detector.ObjectsDetected += OnObjectsDetected;
    }

    private void OnDisable()
    {
        _detector.ObjectsDetected -= OnObjectsDetected;
    }

    private void OnObjectsDetected(RaycastHit[] hits)
    {
        foreach (var hit in hits)
        {
            if (hit.collider.TryGetComponent(out FooPlayer tentacle))
            {
                _container.Init(tentacle);
                NeedTransit = true;
                break;
            }
        }
    }
}
