using System.Collections;
using UnityEngine;

public class LiftObstacle : ActiveObject
{
    [SerializeField] private ParticleSystem _disableEffect;
    [SerializeField] private GameObject _dangerousObject;

    private Animator _selfAnimator;

    private void Start()
    {
        _selfAnimator = GetComponent<Animator>();
    }

    public override void Action()
    {
        _selfAnimator.enabled = false;

        Instantiate(_disableEffect, _dangerousObject.transform.position, Quaternion.identity);
        _dangerousObject.SetActive(false);
    }
}
