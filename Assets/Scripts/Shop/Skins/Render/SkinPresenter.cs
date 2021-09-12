using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SkinPresenter : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RawImage _preview;
    [SerializeField] private Image _frame;
    [SerializeField] private Image _lockImage;
    [SerializeField] private Image _shadow;
    [SerializeField] private Button _selfButton;
    [Header("Parameters")]
    [SerializeField] private Color _lockFrameColor;
    [SerializeField] private Color _buyedFrameColor;
    [SerializeField] private Color _selectedFrameColor;

    private SkinData _data;

    public event UnityAction<SkinPresenter> Clicked;

    public SkinData Data => _data;

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
        Clicked?.Invoke(this);
    }

    public void RenderLocked(SkinData data)
    {
        _data = data;
        _preview.texture = data.Texture;

        _frame.color = _lockFrameColor;
        _lockImage.enabled = true;
        _shadow.enabled = true;
        _selfButton.interactable = false;
    }

    public void RenderBuyed(SkinData data)
    {
        _data = data;
        _preview.texture = data.Texture;

        _frame.color = _buyedFrameColor;
        _lockImage.enabled = false;
        _shadow.enabled = false;
        _selfButton.interactable = true;
    }

    public void RenderSelected(SkinData data)
    {
        _data = data;
        _preview.texture = data.Texture;

        _frame.color = _selectedFrameColor;
        _lockImage.enabled = false;
        _shadow.enabled = false;
        _selfButton.interactable = true;
    }
}
