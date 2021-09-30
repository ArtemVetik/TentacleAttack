using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SkipLevelButton : MonoBehaviour
{
    [SerializeField] private ClothRewardList _rewardList;
    [SerializeField] private EndGamePanel _endPanel;

    private Button _selfButton;
    private AdSettings _adSettings;

    private void Awake()
    {
        _selfButton = GetComponent<Button>();
        _adSettings = Singleton<AdSettings>.Instance;
    }

    private void OnEnable()
    {
        _selfButton.onClick.AddListener(OnSelfButtonClicked);

        if (_rewardList.ContainsLevel(SceneManager.GetActiveScene().buildIndex))
            gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        _selfButton.onClick.AddListener(OnSelfButtonClicked);
    }

    private void OnSelfButtonClicked()
    {
        _adSettings.UserEarnedReward += OnUserEarnedReward;
        _adSettings.ShowRewarded();
    }

    private void OnUserEarnedReward()
    {
        _adSettings.UserEarnedReward -= OnUserEarnedReward;
        _endPanel.LoadNextWithoutAd();
    }
}