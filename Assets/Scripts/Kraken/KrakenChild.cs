using System.Collections;
using UnityEngine;

public class KrakenChild : MonoBehaviour
{
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
    }

}
