using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogDistance : MonoBehaviour
{
    [SerializeField] private float _rearFogPosition = -22;

    private void FixedUpdate()
    {
        RenderSettings.fogStartDistance = -transform.position.z - _rearFogPosition;
        RenderSettings.fogEndDistance = RenderSettings.fogStartDistance + 15f;
    }
}
