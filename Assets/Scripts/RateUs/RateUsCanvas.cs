using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class RateUsCanvas : MonoBehaviour
{
    [SerializeField] private Button _nopeButton;
    [SerializeField] private Button _sureButton;
    [SerializeField] private Button _closeButton;

    public event UnityAction SureButtonClicked;
    public event UnityAction NopeButtonClicked;

    private void OnEnable()
    {
        _nopeButton.onClick.AddListener(OnNopeButtonClicked);
        _sureButton.onClick.AddListener(OnSureButtonClicked);
        _closeButton.onClick.AddListener(OnCloseButtonClicked);
    }

    private void OnDisable()
    {
        _nopeButton.onClick.RemoveListener(OnNopeButtonClicked);
        _sureButton.onClick.RemoveListener(OnSureButtonClicked);
        _closeButton.onClick.RemoveListener(OnCloseButtonClicked);
    }

    private void OnNopeButtonClicked()
    {
        NopeButtonClicked?.Invoke();
        gameObject.SetActive(false);
    }

    private void OnCloseButtonClicked()
    {
        OnNopeButtonClicked();
    }

    private void OnSureButtonClicked()
    {
#if UNITY_ANDROID
        Application.OpenURL("https://play.google.com/store/apps/details?id=kraken.adventure.squid.puzzle");
#elif UNITY_IOS
        Application.OpenURL("https://apps.apple.com/ru/app/kraken-tentacle-monster/id1589228244");
    #endif

            SureButtonClicked?.Invoke();
        gameObject.SetActive(false);
    }
}
