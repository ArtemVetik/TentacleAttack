using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Obstacle))]
[RequireComponent(typeof(AudioSource))]
public class ObstacleSound : MonoBehaviour
{
    private Obstacle _obstacle;
    private AudioSource _audio;

    private void Awake()
    {
        _obstacle = GetComponent<Obstacle>();
        _audio = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        _obstacle.Activated += OnObstacleActivated;
    }

    private void OnDisable()
    {
        _obstacle.Activated -= OnObstacleActivated;
    }

    private void OnObstacleActivated(Obstacle obstacle)
    {
        if (SaveDataBase.GetSoundSetting() == true)
            _audio.Play();
    }
}
