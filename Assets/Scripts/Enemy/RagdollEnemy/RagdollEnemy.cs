using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.FinalIK;

public abstract class RagdollEnemy : MonoBehaviour
{
    [SerializeField] private RagdollUtility _ragdollUtility;

    public void EnableRagdoll()
    {
        _ragdollUtility.EnableRagdoll();
    }
}
