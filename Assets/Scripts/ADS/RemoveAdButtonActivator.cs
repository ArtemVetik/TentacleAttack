using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class RemoveAdButtonActivator : MonoBehaviour
{
    [SerializeField] private IAPButton _removeAdButton;

    private AdSettings _adSettings;

    private void Awake()
    {
        _adSettings = Singleton<AdSettings>.Instance;
    }

    private void OnEnable()
    {
        _adSettings.AdsRemoved += OnAdRemoved;
    }

    private void OnDisable()
    {
        _adSettings.AdsRemoved -= OnAdRemoved;
    }

    private void Start()
    {
        if (_adSettings.IsAdsRemove)
            _removeAdButton.gameObject.SetActive(false);
    }

    private void OnAdRemoved()
    {
        _removeAdButton.gameObject.SetActive(false);
    }
}
