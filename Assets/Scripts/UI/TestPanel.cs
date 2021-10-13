using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TestPanel : MonoBehaviour
{
    [SerializeField] private SkinDataBase _skinDataBase;
    [SerializeField] private AccessoryDataBase _accessoryDataBase;
    [SerializeField] private ClothDataBase _clothDataBase;
    [SerializeField] private Slider _levelSlider;
    [SerializeField] private Text _levelText;

    private void OnEnable()
    {
        _levelSlider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    private void OnDisable()
    {
        _levelSlider.onValueChanged.RemoveListener(OnSliderValueChanged);
    }

    public void ResetRateUsSettings()
    {
        PlayerPrefs.DeleteKey("RateUsCantShowKey");
    }

    private void OnSliderValueChanged(float value)
    {
        _levelText.text = $"Load {value} level";
    }

    public void SelectLevel()
    {
        SaveDataBase.SetCurrentLevel((int)_levelSlider.value - 1);
        SceneManager.LoadScene((int)_levelSlider.value - 1);
    }

    public void AddCoins(int value)
    {
        SaveDataBase.SetScore(SaveDataBase.GetScore() + value);
    }

    public void RemoveCoins(int value)
    {
        var score = SaveDataBase.GetScore();
        if (score < value)
            return;

        SaveDataBase.SetScore(score - value);
    }

    public void ResetSkins()
    {
        var inventory = new SkinInventory(_skinDataBase);
        inventory.Save();
    }

    public void ResetAccessory()
    {
        var inventory = new AccessoryInventory(_accessoryDataBase);
        inventory.Save();
    }

    public void ResetCloth()
    {
        var inventory = new ClothInventory(_clothDataBase);
        inventory.Save();
    }

    public void ResetRemoveAd()
    {
        Singleton<AdSettings>.Instance.ReturnAdsTest();
    }
}
