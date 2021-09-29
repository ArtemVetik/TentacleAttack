using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class BackgroundMusic : MonoBehaviour
{
    [SerializeField] private BackgroundMusicList _musicList;

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
        var sceneName = SceneManager.GetActiveScene().name;
        if (_musicList.TryGetMusicByLevel(sceneName, out AudioClip music))
            _audio.clip = music;
        else
            Debug.LogError("Music for " + sceneName + " not found");

        if (SaveDataBase.GetSoundSetting() == false)
            _audio.Stop();
        else
            _audio.Play();
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
