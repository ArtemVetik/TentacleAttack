using UnityEngine;

[RequireComponent(typeof(Animator))]
public class OpeningWindow : ActiveObject
{
    private Animator _selfAnimator;
    private const string _openWindows = "Opening";
    private const string _closeWindows = "Closing";

    public override void Action()
    {
        if (_selfAnimator == null)
            _selfAnimator = GetComponent<Animator>();

        _selfAnimator.Play(_openWindows);
    }
}

