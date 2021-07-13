using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleDamagedTransition : Transition
{
    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out Obstacle obstacle))
            NeedTransit = true;
    }
}
