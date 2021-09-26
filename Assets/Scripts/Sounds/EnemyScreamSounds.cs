using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class EnemyScreamSounds : MonoBehaviour
{
    [SerializeField] private AudioClip[] _screamSounds;

    private AudioSource _audio;
    private EnemyContainer _enemyContainer;

    private void Awake()
    {
        _audio = GetComponent<AudioSource>();

        _enemyContainer = FindObjectOfType<EnemyContainer>();
    }

    private void OnEnable()
    {
        _enemyContainer.EnemyStucked += OnEnemyStucked;
    }

    private void OnDisable()
    {
        _enemyContainer.EnemyStucked -= OnEnemyStucked;
    }

    private void OnEnemyStucked(Enemy enemy)
    {
        PlayRandomScreamSound();
    }

    private void PlayRandomScreamSound()
    {
        if (SaveDataBase.GetSoundSetting() == false)
            return;

        var randomClip = _screamSounds[Random.Range(0, _screamSounds.Length)];
        _audio.clip = randomClip;
        _audio.Play();
    }
}
