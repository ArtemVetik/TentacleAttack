using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
public class KrakenChild : MonoBehaviour
{
    [SerializeField] private ParticleSystem _foundedEffect;
    [SerializeField] private AudioClip _sound;

    private AudioSource _audio;
    private Animator _selfAnimator;
    private TriggerSpeaker _triggerSpeaker;
    private const string Happy = nameof(Happy);

    public event UnityAction Released;

    private void Awake()
    {
        _triggerSpeaker = GetComponentInChildren<TriggerSpeaker>();
        _selfAnimator = GetComponentInChildren<Animator>();
        _audio = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        _triggerSpeaker.TriggerEnter += OnTriggerSpeakerEnter;
    }

    private void OnDisable()
    {
        _triggerSpeaker.TriggerEnter -= OnTriggerSpeakerEnter;
    }

    private void OnTriggerSpeakerEnter()
    {
        Released?.Invoke();
        GlobalEventStorage.GameOveringInvoke(true);
        _selfAnimator.SetTrigger(Happy);

        if (SaveDataBase.GetSoundSetting() == true)
        {
            _audio.clip = _sound;
            _audio.volume = 0.1f;
            _audio.Play();
        }

        Instantiate(_foundedEffect, transform.position, _foundedEffect.transform.rotation);
    }

}
