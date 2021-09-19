using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdsButtonActivator : MonoBehaviour
{
    [SerializeField] private bool _activateAfterStart = false;
    [SerializeField] private Button[] _adButtons;

    private AdSettings _adSettings;
    private StartMovementTrigger _startTrigger;

    private void Awake()
    {
        SetButtonsActive(false);
        _adSettings = Singleton<AdSettings>.Instance;

        if (_activateAfterStart)
            _startTrigger = FindObjectOfType<StartMovementTrigger>();
    }

    private void OnEnable()
    {
        _adSettings.RewardedLoaded += OnRewardedAdLoaded;
    }

    private void OnDisable()
    {
        _adSettings.RewardedLoaded -= OnRewardedAdLoaded;
    }

    private void OnRewardedAdLoaded()
    {
        StartCoroutine(ActivateButton(0.5f));
    }

    private IEnumerator ActivateButton(float delay)
    {
        if (_activateAfterStart)
            yield return new WaitUntil(() => _startTrigger.IsStarted == true);

        yield return new WaitForSeconds(delay);
        SetButtonsActive(true);
    }

    private void SetButtonsActive(bool isActive)
    {
        foreach (var button in _adButtons)
            button.gameObject.SetActive(isActive);
    }
}
