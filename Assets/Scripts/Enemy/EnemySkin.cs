using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkin : MonoBehaviour
{
    public SkinnedMeshRenderer SkinnedMeshRenderer { get; private set; }

    private void Awake()
    {
        SkinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
    }
}
