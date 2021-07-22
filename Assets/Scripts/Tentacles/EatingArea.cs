using System;
using UnityEngine;

public class EatingArea : MonoBehaviour
{
    [SerializeField] private ParticleSystem _eatingEffect;
    [SerializeField] private Transform _eatingContainer;

    public event Action Eating;

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out Enemy enemy))
        {
            Instantiate(_eatingEffect, _eatingContainer.position, Quaternion.identity);
            Eating?.Invoke();
            Destroy(enemy.gameObject);
        }
    }

}
