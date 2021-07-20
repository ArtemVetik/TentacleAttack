using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class EatingSound : MonoBehaviour
{
    [SerializeField] private AudioClip[] _sounds;

    private AudioSource _audio;

    private void Awake()
    {
        _audio = GetComponent<AudioSource>();
    }

    private void Start()
    {
        var randomClip = _sounds[Random.Range(0, _sounds.Length)];
        _audio.clip = randomClip;

        _audio.Play();
    }
}
