using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AppMetricaEventSender : MonoBehaviour
{
    public enum LevelDiff { Easy, Medium, Hard, }
    public enum LevelType { Normal, Bonus, }

    [SerializeField] private StartMovementTrigger _startTrigger;
    [SerializeField] private LevelDiff _levelDiff;
    [SerializeField] private LevelType _levelType;

    private const string GameMode = "classic";
    private const string LevelCountKey = nameof(LevelCountKey);

    private DateTime _startLevelTime;
    private bool _isStarted;

    private void OnEnable()
    {
        _startTrigger.MoveStarted += OnLevelStart;
        GlobalEventStorage.GameEnded += OnLevelFinished;
    }

    private void OnDisable()
    {
        _startTrigger.MoveStarted -= OnLevelStart;
        GlobalEventStorage.GameEnded -= OnLevelFinished;
    }

    private void OnLevelStart()
    {
        int levelCount = 1;
        if (PlayerPrefs.HasKey(LevelCountKey))
            levelCount = PlayerPrefs.GetInt(LevelCountKey) + 1;

        PlayerPrefs.SetInt(LevelCountKey, levelCount);

        var levelNumber = SceneManager.GetActiveScene().buildIndex + 1;
        var levelName = SceneManager.GetActiveScene().name;
        var levelDiff = _levelDiff.ToString();
        var levelLoop = SaveDataBase.GetLevelLoopCount();
        var levelRandom = 0;
        var levelType = _levelType.ToString();
        var gameMode = GameMode;

        Dictionary<string, object> eventParameters = new Dictionary<string, object>();
        eventParameters.Add("level_number", levelNumber);
        eventParameters.Add("level_name", levelName);
        eventParameters.Add("level_count", levelCount);
        eventParameters.Add("level_diff", levelDiff);
        eventParameters.Add("level_loop", levelLoop);
        eventParameters.Add("level_random", levelRandom);
        eventParameters.Add("level_type", levelType);
        eventParameters.Add("game_mode", gameMode);

        AppMetrica.Instance.ReportEvent("level_start", eventParameters);
        AppMetrica.Instance.SendEventsBuffer();

        _startLevelTime = DateTime.Now;
        _isStarted = true;
    }

    private void OnLevelFinished(bool isLeave, bool isWin, int progress)
    {
        var levelNumber = SceneManager.GetActiveScene().buildIndex + 1;
        var levelName = SceneManager.GetActiveScene().name;
        var levelCount = PlayerPrefs.GetInt(LevelCountKey);
        var levelDiff = _levelDiff.ToString();
        var levelLoop = SaveDataBase.GetLevelLoopCount();
        var levelRandom = 0;
        var levelType = _levelType.ToString();
        var gameMode = GameMode;
        var result = isLeave ? "leave" : isWin ? "win" : "loose";
        var timeSec = (DateTime.Now - _startLevelTime).Seconds;

        Dictionary<string, object> eventParameters = new Dictionary<string, object>();
        eventParameters.Add("level_number", levelNumber);
        eventParameters.Add("level_name", levelName);
        eventParameters.Add("level_count", levelCount);
        eventParameters.Add("level_diff", levelDiff);
        eventParameters.Add("level_loop", levelLoop);
        eventParameters.Add("level_random", levelRandom);
        eventParameters.Add("level_type", levelType);
        eventParameters.Add("game_mode", gameMode);
        eventParameters.Add("result", result);
        eventParameters.Add("time", timeSec);
        eventParameters.Add("progress", progress);
        eventParameters.Add("continue", 1);

        AppMetrica.Instance.ReportEvent("level_finish", eventParameters);
        AppMetrica.Instance.SendEventsBuffer();

        _isStarted = false;
    }

    private void OnLevelFinished(bool isWin, int progress)
    {
        OnLevelFinished(false, isWin, progress);
    }

    private void OnApplicationQuit()
    {
        Debug.Log("OnAppQuit1 " + _isStarted);
        if (_isStarted == false)
            return;

        var endGamePanel = FindObjectOfType<EndGamePanel>();
        var progress = endGamePanel.GetProgress(false);

        OnLevelFinished(true, false, progress);

        Debug.Log("OnAppQuit2");
    }
}
