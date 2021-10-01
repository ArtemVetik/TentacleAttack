using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(Image))]
public class BuyButton : MonoBehaviour
{
    [SerializeField] private int _price;
    [SerializeField] private TMP_Text _buttonText;
    [SerializeField] private Sprite _lockSprite;
    [SerializeField] private Sprite _unlockSprite;

    public event UnityAction BuyConfirmed;

    private Button _selfButton;
    private TMP_Text[] _childTexts;

    private void Awake()
    {
        _selfButton = GetComponent<Button>();
        _childTexts = GetComponentsInChildren<TMP_Text>();
    }

    private void Start()
    {
        _buttonText.text = _price.ToString();
    }

    private void OnEnable()
    {
        SaveDataBase.ScoreChanged += OnScoreChanged;
        _selfButton.onClick.AddListener(OnSelfButtonClicked);

        UpdateButtonView();
    }

    private void OnDisable()
    {
        SaveDataBase.ScoreChanged -= OnScoreChanged;
        _selfButton.onClick.RemoveListener(OnSelfButtonClicked);
    }

    private void OnScoreChanged(int score)
    {
        UpdateButtonView();
    }

    private void UpdateButtonView()
    {
        if (SaveDataBase.GetScore() >= _price)
        {
            _selfButton.image.sprite = _unlockSprite;
            _selfButton.interactable = true;
            foreach (var text in _childTexts)
                text.color = Color.white;
        }
        else
        {
            _selfButton.image.sprite = _lockSprite;
            _selfButton.interactable = false;
            foreach (var text in _childTexts)
                text.color = Color.gray;
        }
    }

    private void OnSelfButtonClicked()
    {
        if (SaveDataBase.GetScore() < _price)
            return;

        SaveDataBase.SetScore(SaveDataBase.GetScore() - _price);
        UpdateButtonView();

        BuyConfirmed?.Invoke();
    }
}
