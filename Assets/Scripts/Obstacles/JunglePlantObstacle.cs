using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class JunglePlantObstacle : Obstacle
{
    [SerializeField] private ParticleSystem _attackEffect;
    [SerializeField] private bool _isSide;

    private static class AnimationParameter
    {
        public static readonly string IsSide = nameof(IsSide);
        public static readonly string Attack = nameof(Attack);
        public static readonly string Dead = nameof(Dead);
    }

    private Animator _selfAnimator;

    private void Awake()
    {
        _selfAnimator = GetComponent<Animator>();
    }

    private void Start()
    {
        _selfAnimator.SetBool(AnimationParameter.IsSide, _isSide);
    }

    protected override void Activate()
    {
        _selfAnimator.SetTrigger(AnimationParameter.Attack);
        Instantiate(_attackEffect, transform.position + Vector3.up, Quaternion.identity);
    }
}
