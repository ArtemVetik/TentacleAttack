using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class AccessoryPresenter : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Image _preview;
    [SerializeField] private Image _frame;
    [SerializeField] private Image _lockImage;
    [SerializeField] private Image _shadow;
    [SerializeField] private Button _selfButton;
    [Header("Parameters")]
    [SerializeField] private Color _lockFrameColor;
    [SerializeField] private Color _buyedFrameColor;
    [SerializeField] private Color _selectedFrameColor;

    private AccessoryData _data;

    public event UnityAction<AccessoryPresenter> Clicked;

    public AccessoryData Data => _data;
    public bool IsRenderSelected { get; private set; }

    private void OnEnable()
    {
        _selfButton.onClick.AddListener(OnSelfButtonClicked);
        IsRenderSelected = false;
    }

    private void OnDisable()
    {
        _selfButton.onClick.RemoveListener(OnSelfButtonClicked);
    }

    private void OnSelfButtonClicked()
    {
        Clicked?.Invoke(this);
    }

    public void RenderLocked(AccessoryData data)
    {
        _data = data;
        _preview.sprite = data.Preview;

        _frame.color = _lockFrameColor;
        _lockImage.enabled = true;
        _shadow.enabled = true;
        _selfButton.interactable = false;
        IsRenderSelected = false;
    }

    public void RenderBuyed(AccessoryData data)
    {
        _data = data;
        _preview.sprite = data.Preview;

        _frame.color = _buyedFrameColor;
        _lockImage.enabled = false;
        _shadow.enabled = false;
        _selfButton.interactable = true;
        IsRenderSelected = false;
    }

    public void RenderSelected(AccessoryData data)
    {
        _data = data;
        _preview.sprite = data.Preview;

        _frame.color = _selectedFrameColor;
        _lockImage.enabled = false;
        _shadow.enabled = false;
        _selfButton.interactable = true;
        IsRenderSelected = true;
    }
}
