using System;
using UnityEngine;

public class EatingArea : MonoBehaviour
{
    public event Action Eating;

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out Enemy enemy))
        {
            Eating?.Invoke();
            Destroy(enemy.gameObject);
        }
    }

}
