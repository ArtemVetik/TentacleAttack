using UnityEngine;
using System.Collections;
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
        StartCoroutine(PresentScore(_score.Value));
    }

    private IEnumerator PresentScore(int value)
    {
        var delay = new WaitForSeconds(2f / value);

        int score = 0;
        SetScoreText(score);
        while (score < value)
        {
            yield return delay;
            SetScoreText(++score);
        }
    }

    private void SetScoreText(int score)
    {
        _scoreText.text = "+ " + score.ToString();
    }
}
