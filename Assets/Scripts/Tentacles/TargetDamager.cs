using System;
using System.Collections;
using UnityEngine;

public class TargetDamager : MonoBehaviour
{
    public event Action<Enemy> EnemyFounded;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Enemy enemy))
        {
            enemy.ApplyDamage();
            EnemyFounded?.Invoke(enemy);
        }
    }
}
