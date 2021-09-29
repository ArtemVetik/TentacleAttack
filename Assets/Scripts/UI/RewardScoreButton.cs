using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class RewardScoreButton : MonoBehaviour
{
    [SerializeField] private int _rewardScoreValue = 200;
    [SerializeField] private TMP_Text _buttonText;

    private Button _selfButton;
    private AdSettings _adSettings;

    private void Awake()
    {
        _selfButton = GetComponent<Button>();
        _adSettings = Singleton<AdSettings>.Instance;
    }

    private void Start()
    {
        _buttonText.text = $"+{_rewardScoreValue}";
    }

    private void OnEnable()
    {
        _selfButton.onClick.AddListener(OnSelfButtonClicked);
    }

    private void OnDisable()
    {
        _selfButton.onClick.RemoveListener(OnSelfButtonClicked);
    }

    private void OnSelfButtonClicked()
    {
        if (_adSettings.IsRewardLoad)
        {
            _adSettings.UserEarnedReward += OnUserEarnedReward;
            _adSettings.ShowRewarded();
        }
    }

    private void OnUserEarnedReward()
    {
        _adSettings.UserEarnedReward -= OnUserEarnedReward;

        var score = SaveDataBase.GetScore();
        SaveDataBase.SetScore(score + _rewardScoreValue);
    }
}
