using System;
using UnityEngine;

public class EatingArea : MonoBehaviour
{
    [SerializeField] private ParticleSystem _eatingEffect;

    public event Action Eating;

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out Enemy enemy))
        {
            Instantiate(_eatingEffect, other.ClosestPoint(enemy.transform.position) + Vector3.up * 2.5f - Vector3.forward * 5f, Quaternion.identity);

            Eating?.Invoke();
            Destroy(enemy.gameObject);
        }
    }

}
