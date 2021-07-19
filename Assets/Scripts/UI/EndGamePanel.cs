using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class EndGamePanel : MonoBehaviour
{
    [SerializeField] private string _winMessage;
    [SerializeField] private string _looseMissage;

    [SerializeField] private TMP_Text _endMessage;
    [SerializeField] private TMP_Text _scoreView;
    [SerializeField] private Button _repeat;
    [SerializeField] private Button _nextLevel;
    [SerializeField] private Color _winColor;
    [SerializeField] private Color _looseColor;

    [SerializeField] private float _delayTime;

    private Animator _selfAnimator;
    private EnemyContainer _enemyContainer;
    private SplineMovement _spline;
    private bool _isLastLevel;
    private const string _openPanel = "OpeningPanel";

    private void Awake()
    {
        _enemyContainer = FindObjectOfType<EnemyContainer>();
    }

    private void OnEnable()
    {
        GlobalEventStorage.GameOvering += OnLevelCompleat;

        _repeat.onClick.AddListener(RepeatScene);
        _nextLevel.onClick.AddListener(NextScene);
    }

    private void OnDisable()
    {
        GlobalEventStorage.GameOvering -= OnLevelCompleat;
        _spline.FullRewinded -= OnLevelCompleat;

        _repeat.onClick.RemoveListener(RepeatScene);
        _nextLevel.onClick.RemoveListener(NextScene);
    }

    private void Start()
    {
        _selfAnimator = GetComponent<Animator>();
        _isLastLevel = FindObjectOfType<KrakenChild>();
        _spline = FindObjectOfType<SplineMovement>();

        if (!_isLastLevel)
            _spline.FullRewinded += OnLevelCompleat;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            PlayerPrefs.DeleteAll();
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

        SaveDataBase.SetCurrentLevel(index);
        SceneManager.LoadScene(index);
    }

    private void OnLevelCompleat(bool isWin)
    {
        if (EndGameConditions(isWin))
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

        _nextLevel.gameObject.SetActive(isWin);
        _scoreView.gameObject.SetActive(isWin);

        _selfAnimator.Play(_openPanel);
    }

    private bool EndGameConditions(bool isWin)
    {
        return !isWin || _isLastLevel || _enemyContainer.AliveEnemyCount == 0;
    }
}