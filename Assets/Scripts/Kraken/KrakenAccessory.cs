using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class KrakenAccessory : MonoBehaviour
{
    private Animator _animator;

    public Animator Animator => _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void Init(AnimatorStateInfo stateInfo, int layerIndex)
    {
        _animator.Play(stateInfo.fullPathHash, layerIndex, stateInfo.normalizedTime);
    }
}
