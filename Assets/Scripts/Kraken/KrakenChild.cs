using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class KrakenChild : MonoBehaviour
{
    [SerializeField] private ParticleSystem _foundedEffect;

    public event UnityAction Released;

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
        Released?.Invoke();
        GlobalEventStorage.GameOveringInvoke(true);
        _selfAnimator.SetTrigger(Happy);

        Instantiate(_foundedEffect, transform.position, _foundedEffect.transform.rotation);
    }

}
