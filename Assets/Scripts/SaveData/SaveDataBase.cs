using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SaveDataBase
{
    private const string ScoreKey = nameof(ScoreKey);
    private const string CurrentLevel = nameof(CurrentLevel);
    private const string SoundSetting = nameof(SoundSetting);
    private const string VibrationSetting = nameof(VibrationSetting);
    private const string LevelLoopCount = nameof(LevelLoopCount);

    public static int GetScore()
    {
        if (PlayerPrefs.HasKey(ScoreKey) == false)
            return 0;

        return PlayerPrefs.GetInt(ScoreKey);
    }

    public static void SetScore(int value)
    {
        PlayerPrefs.SetInt(ScoreKey, value);
    }

    public static int GetCurrentLevelIndex()
    {
        if (PlayerPrefs.HasKey(CurrentLevel) == false)
            return 0;

        return PlayerPrefs.GetInt(CurrentLevel);
    }

    public static void SetCurrentLevel(int buildIndex)
    {
        PlayerPrefs.SetInt(CurrentLevel, buildIndex);
    }

    public static void SetSound(bool isActive)
    {
        PlayerPrefs.SetInt(SoundSetting, isActive ? 1 : 0);
    }

    public static bool GetSoundSetting()
    {
        if (PlayerPrefs.HasKey(SoundSetting) == false)
            return true;

        return PlayerPrefs.GetInt(SoundSetting) == 1;
    }

    public static void SetVibration(bool isActive)
    {
        PlayerPrefs.SetInt(VibrationSetting, isActive ? 1 : 0);
    }

    public static bool GetVibrationSetting()
    {
        if (PlayerPrefs.HasKey(VibrationSetting) == false)
            return true;

        return PlayerPrefs.GetInt(VibrationSetting) == 1;
    }

    public static void AddLevelLoopCount()
    {
        var levelLoop = GetLevelLoopCount();
        PlayerPrefs.SetInt(LevelLoopCount, levelLoop + 1);
    }

    public static int GetLevelLoopCount()
    {
        if (PlayerPrefs.HasKey(LevelLoopCount) == false)
            return 1;

        return PlayerPrefs.GetInt(LevelLoopCount);
    }
}