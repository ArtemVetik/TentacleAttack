using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class WindowBlinds : ActiveObject
{

    private Animator _selfAnimator;
    private readonly string _usingAnimation = "Using";

    private void Start()
    {
        _selfAnimator = GetComponent<Animator>();
    }

    public override void Action()
    {
        _selfAnimator.Play(_usingAnimation);
    }
}