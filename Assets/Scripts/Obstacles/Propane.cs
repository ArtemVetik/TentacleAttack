using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Propane : MonoBehaviour
{
    [SerializeField] private ParticleSystem _smoke;
    [SerializeField] private Transform _smokeContainer;

    private AudioSource _audio;
    private bool _activated = false;

    private void Awake()
    {
        _audio = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_activated == false && other.TryGetComponent(out TargetMovement tentacle))
        {
            if (SaveDataBase.GetSoundSetting() == true)
                _audio.Play();

            Instantiate(_smoke, _smokeContainer.position, _smoke.transform.rotation);
            _activated = true;
        }
    }
}
