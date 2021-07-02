using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HidingSawTrap : ActiveObject
{
    public enum HideSide
    {
        HideX, HideY, HideZ, HideXRev, HideYRev, HideZRev,
    }

    [SerializeField] private HideSide _hideSide;
    
    private Animator _selfAnimator;

    private void Awake()
    {
        _selfAnimator = GetComponent<Animator>();
    }

    public override void Action()
    {
        string stringValue = Enum.GetName(typeof(HideSide), _hideSide);
        _selfAnimator.SetTrigger(stringValue);
    }
}
