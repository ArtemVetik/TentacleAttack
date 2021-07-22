using System.Text;
using System.Collections;
using UnityEngine;
using TMPro;

public class CoinsCounterView : MonoBehaviour
{
    [SerializeField] private LevelScore _score;
    [SerializeField] private float _animationTime;

    private TMP_Text _scoreView;
    private Coroutine _coinsCounter;

    private void Start()
    {
        _scoreView = GetComponentInChildren<TMP_Text>();

        _coinsCounter = StartCoroutine(CoinsCounterViewing());
    }

    private IEnumerator CoinsCounterViewing()
    {
        float score = _score.Value;
        float step = score / _animationTime;
        _scoreView.SetText("+ " + score.ToString("0.#"));

        while (true)
        {
            score -= step * Time.deltaTime;
            if(score > 0)
            {
                _scoreView.SetText("+ " + ((int)score).ToString("0.#"));
            }
            else
            {
                _scoreView.SetText("0");
                //StopCoroutine(_coinsCounter);
                break;
            }

            yield return null;
        }
    }
}
