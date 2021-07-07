using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Sword : MonoBehaviour
{
    [SerializeField] private ParticleSystem _hitEffect;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out TentacleSegment segment))
        {
            GlobalEventStorage.TentacleAddDamageInvoke(segment);
            Instantiate(_hitEffect, segment.MeshCenterPosition, Quaternion.identity);
        }
    }
}