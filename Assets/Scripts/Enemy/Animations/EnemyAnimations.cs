using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EnemyAnimations : MonoBehaviour
{
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void EnableAttack()
    {
        _animator.SetBool(EnemyAnimationParameter.Attacking, true);
    }

    public void DisableAttack()
    {
        _animator.SetBool(EnemyAnimationParameter.Attacking, false);
    }

    public void SetAttackForce(float value)
    {
        _animator.SetFloat(EnemyAnimationParameter.AttackForce, value);
    }

    public void EnemyStuck()
    {
        _animator.SetBool(EnemyAnimationParameter.Teeter, true);
    }

    public void EnemyAnxiety()
    {
        _animator.SetTrigger(EnemyAnimationParameter.Anxiety);
    }

    public void PlayIdle()
    {
        _animator.SetTrigger(EnemyAnimationParameter.Idle);
    }

    public void PlayRotating()
    {
        _animator.SetBool(EnemyAnimationParameter.Rotating, true);
    }

    public void StopRotating()
    {
        _animator.SetBool(EnemyAnimationParameter.Rotating, false);
    }
}
