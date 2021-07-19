using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class EndGamePanel : MonoBehaviour
{
    [SerializeField] private GameObject _winWindow;
    [SerializeField] private GameObject _looseWindow;
    [SerializeField] private Animator _topPanel;
    [SerializeField] private float _delayTime;

    private EnemyContainer _enemyContainer;
    private SplineMovement _spline;
    private bool _isLastLevel;

    private void Awake()
    {
        _enemyContainer = FindObjectOfType<EnemyContainer>();
    }

    private void OnEnable()
    {
        GlobalEventStorage.GameOvering += OnLevelCompleat;
    }

    private void OnDisable()
    {
        GlobalEventStorage.GameOvering -= OnLevelCompleat;
        _spline.FullRewinded -= OnLevelCompleat;
    }

    private void Start()
    {
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

    public void RepeatScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void NextScene()
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
        _topPanel.SetTrigger("Hide");

        yield return new WaitForSeconds(_delayTime);

        if (isWin)
            _winWindow.SetActive(true);
        else
            _looseWindow.SetActive(true);
    }

    private bool EndGameConditions(bool isWin)
    {
        return !isWin || _isLastLevel || _enemyContainer.AliveEnemyCount == 0;
    }
}