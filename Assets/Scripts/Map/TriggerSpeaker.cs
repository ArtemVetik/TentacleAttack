using System;
using System.Collections;
using UnityEngine;

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
