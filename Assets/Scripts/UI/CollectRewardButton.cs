using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class CollectRewardButton : MonoBehaviour
{
    [SerializeField] private EndGamePanel _endPanel;
    [SerializeField] private LevelScore _levelScore;
    [SerializeField] private TMP_Text _buttonText;

    private readonly string ScoreMultiplyKey = nameof(ScoreMultiplyKey);

    private Button _selfButton;
    private AdSettings _adSettings;
    private ScoreMultiply _scoreMultiply;

    private void Awake()
    {
        _selfButton = GetComponent<Button>();
        _adSettings = Singleton<AdSettings>.Instance;
    }

    private void Start()
    {
        _scoreMultiply = new ScoreMultiply();
        _buttonText.text = $"Collect x{_scoreMultiply.Value}";
    }

    private void OnEnable()
    {
        _endPanel.ScoreCollected += OnScoreCollected;
        _selfButton.onClick.AddListener(OnSelfButtonClicked);
    }

    private void OnDisable()
    {
        _endPanel.ScoreCollected -= OnScoreCollected;
        _selfButton.onClick.RemoveListener(OnSelfButtonClicked);
    }


    private void OnUserEarnedReward()
    {
        _adSettings.UserEarnedReward -= OnUserEarnedReward;

        var score = SaveDataBase.GetScore();
        var levelScore = _levelScore.Value * _scoreMultiply.Value;
        SaveDataBase.SetScore(score + levelScore);

        _scoreMultiply.Reset();
        _endPanel.LoadNextWithoutAd();
    }

    private void OnScoreCollected()
    {
        _scoreMultiply.IncreaseMultiply();
    }

    private void OnSelfButtonClicked()
    {
        _adSettings.UserEarnedReward += OnUserEarnedReward;
        _adSettings.ShowRewarded();
    }
}

public class ScoreMultiply
{
    public int Value { get; private set; }

    private readonly int[] MultiplierValues = new int[] { 2, 3, 5, 7 };
    private readonly string ScoreMultiplyKey = nameof(ScoreMultiplyKey);

    public ScoreMultiply()
    {
        if (PlayerPrefs.HasKey(ScoreMultiplyKey))
            Value = PlayerPrefs.GetInt(ScoreMultiplyKey);
        else
            Value = MultiplierValues[0];
    }

    public void IncreaseMultiply()
    {
        var index = GetMultuplyIndex(Value);
        index = Mathf.Clamp(index + 1, 0, MultiplierValues.Length - 1);

        PlayerPrefs.SetInt(ScoreMultiplyKey, MultiplierValues[index]);
    }

    public void Reset()
    {
        Value = MultiplierValues[0];
        PlayerPrefs.SetInt(ScoreMultiplyKey, Value);
    }

    private int GetMultuplyIndex(int value)
    {
        for (int i = 0; i < MultiplierValues.Length; i++)
            if (MultiplierValues[i] == value)
                return i;

        throw new ArgumentOutOfRangeException($"Can't find value {value} in MultiplyValues array");
    }
}
