using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndClothReward : MonoBehaviour
{
    [SerializeField] private GameObject _rewardedGroup;
    [SerializeField] private Button _rewardButton;
    [SerializeField] private ClothDataBase _dataBase;
    [SerializeField] private ClothRewardList _rewardData;
    [SerializeField] private Image _clothPreview;

    private ClothRewardData _reward;
    private AdSettings _adSettings;
    private ClothData _unlockData;

    private void Awake()
    {
        _adSettings = Singleton<AdSettings>.Instance;
        _rewardedGroup.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        _rewardButton.onClick.AddListener(OnRewardButtonClicked);
        GlobalEventStorage.GameEnded += OnGameEnded;
    }

    private void OnDisable()
    {
        _rewardButton.onClick.RemoveListener(OnRewardButtonClicked);
        GlobalEventStorage.GameEnded -= OnGameEnded;
    }

    private void OnGameEnded(bool isWin, int progress)
    {
        var levelIndex = SceneManager.GetActiveScene().buildIndex;
        if (_rewardData.TryGetByLevel(levelIndex, out _reward))
        {
            _rewardedGroup.gameObject.SetActive(true);
            var unlockIndex = _reward.ClothDataBaseIndex;
            _unlockData = _dataBase[unlockIndex];
            _clothPreview.sprite = _unlockData.Preview;

            var inventory = new ClothInventory(_dataBase);
            inventory.Load();
            inventory.AddAvailable(_unlockData);
            inventory.Save();
        }
    }

    private void OnRewardButtonClicked()
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

        var inventory = new ClothInventory(_dataBase);
        inventory.Load();
        inventory.Add(_unlockData);
        inventory.Save();

        _rewardedGroup.SetActive(false);
    }
}
