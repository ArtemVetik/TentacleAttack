using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Events;

public class EndGamePanel : MonoBehaviour
{
    [SerializeField] private GameObject _winWindow;
    [SerializeField] private GameObject _looseWindow;
    [SerializeField] private Animator _topPanel;
    [SerializeField] private Button _collectButton;
    [SerializeField] private LevelScore _levelScore;
    [SerializeField] private float _delayTime;

    public event UnityAction ScoreCollected;

    private EnemyContainer _enemyContainer;
    private SplineMovement _spline;
    private AdSettings _adSettings;
    private int _nextSceneIndex;
    private bool _isLastLevel;

    private void Awake()
    {
        _enemyContainer = FindObjectOfType<EnemyContainer>();
        _adSettings = Singleton<AdSettings>.Instance;
    }

    private void OnEnable()
    {
        _isLastLevel = FindObjectOfType<KrakenChild>();
        _spline = FindObjectOfType<SplineMovement>();

        if (!_isLastLevel)
            _spline.FullRewinded += OnLevelCompleat;

        GlobalEventStorage.GameOvering += OnLevelCompleat;
        _collectButton.onClick.AddListener(OnCollectButtonClicked);
        _adSettings.InterstitialShowed += OnInterstitialShowed;
        _adSettings.InterstitialShowTryed += OnInterstitialShowed;
    }
    
    private void OnDisable()
    {
        GlobalEventStorage.GameOvering -= OnLevelCompleat;
        _spline.FullRewinded -= OnLevelCompleat;
        _collectButton.onClick.RemoveListener(OnCollectButtonClicked);
        _adSettings.InterstitialShowed -= OnInterstitialShowed;
        _adSettings.InterstitialShowTryed -= OnInterstitialShowed;
    }

    private void OnInterstitialShowed()
    {
        SceneManager.LoadScene(_nextSceneIndex);
    }

    public void RepeatScene()
    {
        _nextSceneIndex = SceneManager.GetActiveScene().buildIndex;
        _adSettings.ShowInterstitial();
    }

    private void OnCollectButtonClicked()
    {
        var score = SaveDataBase.GetScore();
        SaveDataBase.SetScore(score + _levelScore.Value);
        ScoreCollected?.Invoke();

        NextScene();
    }

    public void NextScene()
    {
        int index = SceneManager.GetActiveScene().buildIndex;
        int sceneCount = SceneManager.sceneCountInBuildSettings;
        index++;
        if (index >= sceneCount)
        {
            SaveDataBase.AddLevelLoopCount();
            index = 0;
        }

        SaveDataBase.SetCurrentLevel(index);
        _nextSceneIndex = index;

        _adSettings.ShowInterstitial();
    }

    public void LoadNextWithoutAd()
    {
        int index = SceneManager.GetActiveScene().buildIndex;
        int sceneCount = SceneManager.sceneCountInBuildSettings;
        index++;
        if (index >= sceneCount)
        {
            SaveDataBase.AddLevelLoopCount();
            index = 0;
        }

        SaveDataBase.SetCurrentLevel(index);
        SceneManager.LoadScene(index);
    }

    private void OnLevelCompleat(bool isWin)
    {
        if (EndGameConditions(isWin))
        {
            StartCoroutine(ShowEndPanel(isWin));
            GlobalEventStorage.GameEndedInvoke(isWin, GetProgress(isWin));
        }
    }

    private IEnumerator ShowEndPanel(bool isWin)
    {
        _topPanel.SetTrigger("Hide");

        yield return new WaitForSeconds(_delayTime);

        StartCoroutine(ShowContinueButton(2f));
        if (isWin)
            _winWindow.SetActive(true);
        else
            _looseWindow.SetActive(true);
    }

    public IEnumerator ShowContinueButton(float delay)
    {
        _collectButton.gameObject.SetActive(false);
        yield return new WaitForSeconds(delay);
        _collectButton.gameObject.SetActive(true);
    }

    private bool EndGameConditions(bool isWin)
    {
        return !isWin || _isLastLevel || _enemyContainer.AliveEnemyCount == 0;
    }

    public int GetProgress(bool isWin)
    {
        if (isWin)
            return 100;

        if (_isLastLevel)
        {
            return 50;
        }
        else
        {
            return (int)(100 - (float)_enemyContainer.AliveEnemyCount / _enemyContainer.StartEnemyCount * 100);
        }
    }
}