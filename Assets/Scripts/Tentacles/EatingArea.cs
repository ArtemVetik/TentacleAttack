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
            Instantiate(_eatingEffect, enemy.transform.position + transform.forward * 2f - Vector3.forward * 2f + Vector3.up, Quaternion.identity);

            Eating?.Invoke();
            Destroy(enemy.gameObject);
        }
    }

}
