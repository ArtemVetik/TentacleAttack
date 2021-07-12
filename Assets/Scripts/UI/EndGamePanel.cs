using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class EndGamePanel : MonoBehaviour
{
    [SerializeField] private string _winMessage;
    [SerializeField] private string _looseMissage;
    [SerializeField] private EnemyContainer _enemyContainer;

    [SerializeField] private TMP_Text _endMessage;
    [SerializeField] private Button _repeat;
    [SerializeField] private Button _nextLevel;
    [SerializeField] private Color _winColor;
    [SerializeField] private Color _looseColor;

    [SerializeField] private float _delayTime;

    private Animator _selfAnimator;
    private SplineMovement _spline;
    private const string _openPanel = "OpeningPanel";


    private void OnEnable()
    {
        GlobalEventStorage.GameOvering += OnLevelCompleat;

        _repeat.onClick.AddListener(RepeatScene);
        _nextLevel.onClick.AddListener(NextScene);
    }

    private void OnDisable()
    {
        GlobalEventStorage.GameOvering -= OnLevelCompleat;
        _spline.SplineRewinded -= OnLevelCompleat;

        _repeat.onClick.RemoveListener(RepeatScene);
        _nextLevel.onClick.RemoveListener(NextScene);
    }

    private void Start()
    {
        _selfAnimator = GetComponent<Animator>();
        _spline = FindObjectOfType<SplineMovement>();
        _spline.SplineRewinded += OnLevelCompleat;
    }

    private void RepeatScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void NextScene()
    {
        int index = SceneManager.GetActiveScene().buildIndex;
        int sceneCount = SceneManager.sceneCountInBuildSettings;
        index++;
        if (index >= sceneCount)
            index = 0;
        SceneManager.LoadScene(index);
    }

    private void OnLevelCompleat(bool isWin)
    {
        if (!isWin || _enemyContainer.AliveEnemyCount == 0)
        {
            StartCoroutine(ShowEndPanel(isWin));
            GlobalEventStorage.GameEndedInvoke(isWin);
        }
    }

    private IEnumerator ShowEndPanel(bool isWin)
    {
        yield return new WaitForSeconds(_delayTime);

        _endMessage.SetText(isWin ? _winMessage : _looseMissage);
        _endMessage.color = isWin ? _winColor : _looseColor;
        _selfAnimator.Play(_openPanel);
    }
}