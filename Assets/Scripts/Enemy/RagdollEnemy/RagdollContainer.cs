using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RagdollContainer : MonoBehaviour
{
    public abstract RagdollEnemy InstRagdollEnemy(Vector3 position, Quaternion rotation);
}
