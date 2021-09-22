using UnityEngine;
using UnityEngine.Events;

public abstract class Obstacle : MonoBehaviour
{
    public event UnityAction<Obstacle> Activated;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out TentacleSegment segment))
        {
            GlobalEventStorage.TentacleAddDamageInvoke(segment);
            Activated?.Invoke(this);
            Activate();
        }
    }

    protected abstract void Activate();
}
