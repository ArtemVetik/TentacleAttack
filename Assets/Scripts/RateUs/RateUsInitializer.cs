using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RateUsInitializer : MonoBehaviour
{
    [SerializeField] private RateUsCanvas _template;

    private const string RateUsCantShowKey = nameof(RateUsCantShowKey);
    private const int FirstShowNumber = 6;
    private const int NextShowDelay = 86400;

    private bool CantShow => PlayerPrefs.HasKey(RateUsCantShowKey);

    private SerializableDateTime _lastShowTime;
    private RateUsCanvas _instCanvas;

    private void OnEnable()
    {
        _lastShowTime = new SerializableDateTime();
        _lastShowTime.Load();

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

        _lastShowTime.Save();
    }

    private void Awake()
    {
        var other = FindObjectOfType<RateUsInitializer>();
        if (Equals(other) == false)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadMode)
    {
        if (CantShow)
            return;

        var dateDiff = DateTime.Now.Subtract(_lastShowTime.ToDateTime);
        var delayed = dateDiff.TotalSeconds > NextShowDelay;

        if (delayed == false || scene.buildIndex + 1 < FirstShowNumber)
            return;

        ShowRateUsWindow();
    }

    private void ShowRateUsWindow()
    {
        _instCanvas = Instantiate(_template);

        _instCanvas.NopeButtonClicked += OnNopeButtonClicked;
        _instCanvas.SureButtonClicked += OnSureButtonClicked;
    }

    private void OnNopeButtonClicked()
    {
        _lastShowTime = new SerializableDateTime(DateTime.Now);

        _instCanvas.NopeButtonClicked -= OnNopeButtonClicked;
        _instCanvas.SureButtonClicked -= OnSureButtonClicked;
    }

    private void OnSureButtonClicked()
    {
        PlayerPrefs.SetInt(RateUsCantShowKey, 1);

        _instCanvas.NopeButtonClicked -= OnNopeButtonClicked;
        _instCanvas.SureButtonClicked -= OnSureButtonClicked;
    }
}

[Serializable]
public struct SerializableDateTime
{
    [SerializeField] private int _year;
    [SerializeField] private int _month;
    [SerializeField] private int _day;
    [SerializeField] private int _hour;
    [SerializeField] private int _minute;
    [SerializeField] private int _second;

    public DateTime ToDateTime => new DateTime(_year, _month, _day, _hour, _minute, _second);

    private const string SerializableDateTimeSaveKey = nameof(SerializableDateTimeSaveKey);

    public SerializableDateTime(int year, int month, int day, int hour, int minute, int second)
    {
        _year = year;
        _month = month;
        _day = day;
        _hour = hour;
        _minute = minute;
        _second = second;
    }

    public SerializableDateTime(DateTime dateTime)
        : this(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second)
    { }

    public void Save()
    {
        var jsonString = JsonUtility.ToJson(this);
        PlayerPrefs.SetString(SerializableDateTimeSaveKey, jsonString);
    }

    public void Load()
    {
        var saved = new SerializableDateTime(DateTime.MinValue);
        //if (PlayerPrefs.HasKey(SerializableDateTimeSaveKey))
        //{
        //    var jsonString = PlayerPrefs.GetString(SerializableDateTimeSaveKey);
        //    saved = JsonUtility.FromJson<SerializableDateTime>(jsonString);
        //}

        _year = saved._year;
        _month = saved._month;
        _day = saved._day;
        _hour = saved._hour;
        _minute = saved._minute;
        _second = saved._second;
    }
}
