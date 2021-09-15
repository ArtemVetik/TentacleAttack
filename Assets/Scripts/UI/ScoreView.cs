using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreView : MonoBehaviour
{
    [SerializeField] private TMP_Text _scoreValue;

    private void OnEnable()
    {
        SaveDataBase.ScoreChanged += OnScoreChanged;
    }

    private void OnDisable()
    {
        SaveDataBase.ScoreChanged -= OnScoreChanged;
    }

    private void Start()
    {
        _scoreValue.text = SaveDataBase.GetScore().ToString();
    }

    private void OnScoreChanged(int score)
    {
        _scoreValue.text = score.ToString();
    }
}
