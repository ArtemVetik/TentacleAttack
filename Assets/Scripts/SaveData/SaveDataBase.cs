using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SaveDataBase
{
    private const string ScoreKey = nameof(ScoreKey);
    private const string CurrentLevel = nameof(CurrentLevel);

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
}