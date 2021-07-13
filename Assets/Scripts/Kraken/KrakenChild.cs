using System.Collections;
using UnityEngine;

public class KrakenChild : MonoBehaviour
{
    [SerializeField] private ParticleSystem _findedEffect;

    private Animator _selfAnimator;
    private TriggerSpeaker _triggerSpeaker;
    private const string Happy = nameof(Happy);

    private void Start()
    {
        _selfAnimator = GetComponentInChildren<Animator>();
        _triggerSpeaker = GetComponentInChildren<TriggerSpeaker>();
        _triggerSpeaker.TriggerEnter += OnTriggerSpeakerEnter;
    }

    private void OnTriggerSpeakerEnter()
    {
        GlobalEventStorage.GameOveringInvoke(true);
        _selfAnimator.SetTrigger(Happy);

        Instantiate(_findedEffect, transform.position, _findedEffect.transform.rotation);
    }

}
