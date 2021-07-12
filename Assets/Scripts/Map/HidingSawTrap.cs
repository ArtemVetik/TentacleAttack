using UnityEngine;
using System;
using SplineMesh;
using System.Collections;

public class HidingSawTrap : ActiveObject
{
    public enum HideSide
    {
        HideX, HideY, HideZ, HideXRev, HideYRev, HideZRev,
    }

    [SerializeField] private HideSide _hideSide;
    [SerializeField] private bool _isMoving;
    
    private Animator _selfAnimator;
    private Spline _spline;
    private Coroutine _moving;

    private void Awake()
    {
        _selfAnimator = GetComponent<Animator>();
        _spline = GetComponentInParent<Spline>();

        if(_isMoving && _spline != null)
        {
            _moving = StartCoroutine(Moving());
        }
    }

    public override void Action()
    {
        string stringValue = Enum.GetName(typeof(HideSide), _hideSide);
        _selfAnimator.SetTrigger(stringValue);
    }

    private IEnumerator Moving()
    {
        while(true)
        {

            yield return null;
        }
    }
}
