using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class TriggerSpeaker : MonoBehaviour
{
    public event Action TriggerEnter;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out TargetMovement target))
        {
            TriggerEnter?.Invoke();
        }
    }

}
