using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public event UnityAction Damaged;

    public int Value { get; private set; }

    private void Start()
    {
        Value = 3;
    }

    public bool TakeDamage()
    {
        if (Value <= 0)
            return false;

        Value--;
        Damaged?.Invoke();

        if (Value == 0)
        {
            GlobalEventStorage.GameOveringInvoke(false);
            return false;
        }

        return true;
    }
}
