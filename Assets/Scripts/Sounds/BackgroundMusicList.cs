using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text.RegularExpressions;

[CreateAssetMenu(fileName = "BackgroundMusicList", menuName = "Sound/BackgroundMusicList", order = 51)]
public class BackgroundMusicList : ScriptableObject
{
    [SerializeField] private List<MusicData> _musicDatas = new List<MusicData>();

    public bool TryGetMusicByLevel(string levelName, out AudioClip music)
    {
        foreach (var data in _musicDatas)
        {
            if (Regex.IsMatch(levelName, data.LelvelPattern) == true)
            {
                music = data.Music;
                return true;
            }
        }

        music = null;
        return false;
    }
}

[Serializable]
public class MusicData
{
    [SerializeField] private string _levelPattern;
    [SerializeField] private AudioClip _music;

    public string LelvelPattern => _levelPattern;
    public AudioClip Music => _music;
}
