using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Obstacle : MonoBehaviour
{
    public event UnityAction<Obstacle> Activated;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out TentacleSegment player))
        {
            Activate();
            GlobalEventStorage.TentacleAddDamageInvoke();
            Activated?.Invoke(this);
        }
    }

    protected abstract void Activate();
}
