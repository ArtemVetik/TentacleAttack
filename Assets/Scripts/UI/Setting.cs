using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Setting : MonoBehaviour
{
    [SerializeField] private GameObject _settingPanel;
    [SerializeField] private Button _level1;
    [SerializeField] private Button _level2;
    [SerializeField] private Button _level3;

    private Button _settingBtn;
    private bool _isFilling;

    private void Start()
    {
        _settingBtn = GetComponent<Button>();
        _settingBtn.onClick.AddListener(ShowSettingPanel);

    }

    private void ShowSettingPanel()
    {
        _settingPanel.SetActive(!_settingPanel.activeSelf);

        if (!_isFilling)
        {
            _level1.onClick.AddListener(LoadScene1);
            _level2.onClick.AddListener(LoadScene2);
            _level3.onClick.AddListener(LoadScene3);
            _isFilling = true;
        }
    }

    private void LoadScene1() => LoadScene(0);

    private void LoadScene2() => LoadScene(1);

    private void LoadScene3() => LoadScene(2);

    private void LoadScene(int index)
    {
        SceneManager.LoadScene(index);
    }
}