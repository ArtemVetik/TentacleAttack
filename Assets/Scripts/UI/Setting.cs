using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Button))]
public class Setting : MonoBehaviour
{
    [SerializeField] private GameObject _settingPanel;
    [SerializeField] private Toggle _soundToggle;
    [SerializeField] private Toggle _vibrationToggle;

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
    }

    private void OnDisable()
    {
        _settingBtn.onClick.RemoveListener(ShowSettingPanel);
        _soundToggle.onValueChanged.RemoveListener(OnSoundSettingsChanged);
        _vibrationToggle.onValueChanged.RemoveListener(OnVibrationSettingsChanged);
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
        SceneManager.LoadScene(name);
    }
}