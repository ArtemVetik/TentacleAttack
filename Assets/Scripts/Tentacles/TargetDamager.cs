using System;
using System.Collections;
using UnityEngine;

public class TargetDamager : MonoBehaviour
{
    public event Action<Enemy> EnemyFounded;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.TryGetComponent(out Enemy enemy))
        {
            enemy.ApplyDamage();
            EnemyFounded?.Invoke(enemy);
            Debug.Log("Damage");
        }
    }
}
