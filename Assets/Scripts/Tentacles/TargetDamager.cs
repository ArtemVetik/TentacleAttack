using System;
using System.Collections;
using UnityEngine;

public class TargetDamager : MonoBehaviour
{
    private TargetMovement _targetMovement;

    public event Action<Enemy> EnemyFounded;

    private void Start()
    {
        _targetMovement = GetComponent<TargetMovement>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Enemy enemy))
        {
            if (_targetMovement != null && _targetMovement.IsUsed)
            {
                enemy.ApplyDamage();
                EnemyFounded?.Invoke(enemy);
            }
        }
    }
}
