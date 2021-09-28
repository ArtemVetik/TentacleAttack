using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BackgroundMusic : MonoBehaviour
{
    private AudioSource _audio;

    private void Awake()
    {
        _audio = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        GlobalEventStorage.GameEnded += OnGameEnded;
        SaveDataBase.SoundSettingChanged += OnSoundSettingChanged;
    }

    private void OnDisable()
    {
        GlobalEventStorage.GameEnded -= OnGameEnded;
        SaveDataBase.SoundSettingChanged -= OnSoundSettingChanged;
    }

    private void Start()
    {
        if (SaveDataBase.GetSoundSetting() == false)
            _audio.Stop();
    }

    private void OnSoundSettingChanged(bool soundEnable)
    {
        if (soundEnable)
            _audio.Play();
        else
            _audio.Pause();
    }

    private void OnGameEnded(bool inWin, int progress)
    {
        _audio.Stop();
    }
}
