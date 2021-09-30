using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class KrakenCloth : MonoBehaviour
{
    private Animator _animator;

    public Animator Animator => _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void Init(AnimatorStateInfo stateInfo)
    {
        _animator.Play(stateInfo.fullPathHash, 0, stateInfo.normalizedTime);
    }

    public void SetTrigger(string id)
    {
        _animator.SetTrigger(id);
    }
}
