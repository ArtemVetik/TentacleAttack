using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Text))]
public class LevelScorePresenter : MonoBehaviour
{
    [SerializeField] private LevelScore _score;

    private TMP_Text _scoreText;

    private void Awake()
    {
        _scoreText = GetComponent<TMP_Text>();
    }

    private void OnEnable()
    {
        GlobalEventStorage.GameEnded += OnGameEnded;
    }

    private void OnDisable()
    {
        GlobalEventStorage.GameEnded -= OnGameEnded;
    }

    private void OnGameEnded(bool isWin)
    {
        if (isWin == false)
            return;

        _scoreText.text = _score.Value.ToString();
        _score.SaveScore();
    }
}
