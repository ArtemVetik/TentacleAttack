using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ClothPresenter : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Image _notAviableImage;
    [SerializeField] private Image _preview;
    [SerializeField] private Image _frame;
    [SerializeField] private Image _lockImage;
    [SerializeField] private Image _shadow;
    [SerializeField] private Button _selfButton;
    [SerializeField] private Button _unlockButton;
    [Header("Parameters")]
    [SerializeField] private Sprite _notAviableSprite;

    private ClothData _data;

    public event UnityAction<ClothPresenter> Clicked;
    public event UnityAction<ClothPresenter> ClickUnlocked;

    public ClothData Data => _data;
    public bool IsRenderSelected { get; private set; }

    private void OnEnable()
    {
        _selfButton.onClick.AddListener(OnSelfButtonClicked);
        _unlockButton.onClick.AddListener(OnUnlockButtonClicked);
        IsRenderSelected = false;
    }

    private void OnDisable()
    {
        _selfButton.onClick.RemoveListener(OnSelfButtonClicked);
        _unlockButton.onClick.RemoveListener(OnUnlockButtonClicked);
    }

    private void OnSelfButtonClicked()
    {
        Clicked?.Invoke(this);
    }

    private void OnUnlockButtonClicked()
    {
        ClickUnlocked?.Invoke(this);
    }

    public void RenderLocked(ClothData data)
    {
        _data = data;
        _preview.sprite = data.Preview;

        _notAviableImage.gameObject.SetActive(false);
        _lockImage.enabled = true;
        _shadow.enabled = true;
        _selfButton.gameObject.SetActive(true);
        _unlockButton.gameObject.SetActive(true);
        _frame.enabled = false;
        _selfButton.interactable = false;
        IsRenderSelected = false;
    }

    public void RenderBuyed(ClothData data)
    {
        _data = data;
        _preview.sprite = data.Preview;

        _notAviableImage.gameObject.SetActive(false);
        _lockImage.enabled = false;
        _shadow.enabled = false;
        _selfButton.gameObject.SetActive(true);
        _unlockButton.gameObject.SetActive(false);
        _frame.enabled = false;
        _selfButton.interactable = true;
        IsRenderSelected = false;
    }

    public void RenderSelected(ClothData data)
    {
        _data = data;
        _preview.sprite = data.Preview;

        _notAviableImage.gameObject.SetActive(false);
        _lockImage.enabled = false;
        _shadow.enabled = false;
        _selfButton.gameObject.SetActive(true);
        _unlockButton.gameObject.SetActive(false);
        _frame.enabled = true;
        _selfButton.interactable = true;
        IsRenderSelected = true;
    }

    public void RenderNotAviable(ClothData data)
    {
        _data = data;
        _notAviableImage.gameObject.SetActive(true);
        _notAviableImage.sprite = _notAviableSprite;

        _preview.enabled = false;
        _lockImage.enabled = false;
        _shadow.enabled = false;
        _selfButton.gameObject.SetActive(false);
        _unlockButton.gameObject.SetActive(false);
        _frame.enabled = false;
        IsRenderSelected = false;
    }
}
