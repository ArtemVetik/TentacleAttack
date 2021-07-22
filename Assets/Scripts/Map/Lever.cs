using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Lever : MonoBehaviour
{
    [SerializeField] private TriggerSpeaker _trigger;
    [SerializeField] private ActiveObject[] _activeObjects;
    [SerializeField] private ParticleSystem _pointer;

    private Animator _selfAnimator;
    private bool _isUsed;

    private const string _using = "Using";

    private void OnEnable()
    {
        _trigger.TriggerEnter += OnTriggerSpeakerEnter;
    }

    private void OnDisable()
    {
        _trigger.TriggerEnter -= OnTriggerSpeakerEnter;
    }


    private void OnTriggerSpeakerEnter()
    {
        if (!_isUsed)
        {
            if (_selfAnimator == null)
                _selfAnimator = GetComponent<Animator>();
            _isUsed = true;

            foreach (var activeObject in _activeObjects)
            {
                activeObject.Action();
            }

            _selfAnimator.Play(_using);
            _pointer.Stop();
        }
    }

}
