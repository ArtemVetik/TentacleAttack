using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SkipLevelButton : MonoBehaviour
{
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
        _adSettings.UserEarnedReward += OnUserEarnedReward;
        _selfButton.onClick.AddListener(OnSelfButtonClicked);
    }

    private void OnDestroy()
    {
        _adSettings.UserEarnedReward -= OnUserEarnedReward;
        _selfButton.onClick.AddListener(OnSelfButtonClicked);
    }

    private void OnSelfButtonClicked()
    {
        _adSettings.ShowRewarded();
    }

    private void OnUserEarnedReward()
    {
        _endPanel.NextScene();
    }
}