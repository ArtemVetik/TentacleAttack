using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Button))]
public class Setting : MonoBehaviour
{
    [SerializeField] private EndGamePanel _endPanel;
    [SerializeField] private GameObject _settingPanel;
    [SerializeField] private Toggle _soundToggle;
    [SerializeField] private Toggle _vibrationToggle;
    [SerializeField] private Button _restartButton;

    private Button _settingBtn;

    private void Awake()
    {
        _settingBtn = GetComponent<Button>();
    }

    private void OnEnable()
    {
        _settingBtn.onClick.AddListener(ShowSettingPanel);
        _soundToggle.onValueChanged.AddListener(OnSoundSettingsChanged);
        _vibrationToggle.onValueChanged.AddListener(OnVibrationSettingsChanged);
        _restartButton.onClick.AddListener(OnRestartButtonClicked);
    }

    private void OnDisable()
    {
        _settingBtn.onClick.RemoveListener(ShowSettingPanel);
        _soundToggle.onValueChanged.RemoveListener(OnSoundSettingsChanged);
        _vibrationToggle.onValueChanged.RemoveListener(OnVibrationSettingsChanged);
        _restartButton.onClick.RemoveListener(OnRestartButtonClicked);
    }

    private void Start()
    {
        _soundToggle.isOn = SaveDataBase.GetSoundSetting();
    }

    private void ShowSettingPanel()
    {
        Time.timeScale = 0;
        _settingPanel.SetActive(true);
    }

    public void CloseSettingsPanel()
    {
        Time.timeScale = 1;
        _settingPanel.SetActive(false);
    }

    private void OnRestartButtonClicked()
    {
        Time.timeScale = 1;
        _endPanel.RepeatScene();
    }

    private void OnSoundSettingsChanged(bool isActive)
    {
        SaveDataBase.SetSound(isActive);
    }

    private void OnVibrationSettingsChanged(bool isActive)
    {
        SaveDataBase.SetVibration(isActive);
    }

    public void LoadScene(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void LoadScene(string name)
    {
        Time.timeScale = 1f;
        SaveDataBase.SetCurrentLevel(SceneManager.GetSceneByName(name).buildIndex);
        SceneManager.LoadScene(name);
    }
}