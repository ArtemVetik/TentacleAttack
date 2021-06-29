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
            GlobalEventStorage.TentacleAddDamageInvoke();
            Activated?.Invoke(this);
            Activate();
        }
    }

    protected abstract void Activate();
}
