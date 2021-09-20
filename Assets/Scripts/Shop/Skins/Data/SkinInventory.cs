using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class SkinInventory : IShopSaved
{
    [SerializeField] private List<string> _buyedGUID = new List<string>();
    [SerializeField] private string _selectedGUID;

    public string SaveKey => "SkinInventorySaveKey";
    public SkinData SelectedSkin
    {
        get
        {
            if (string.IsNullOrEmpty(_selectedGUID))
                return null;

            return _dataBase.Data.First((data) => data.GUID == _selectedGUID);
        }
    }
    public IEnumerable<SkinData> Data => from data in _dataBase.Data
                                         where _buyedGUID.Contains(data.GUID)
                                         select data;

    public static event UnityAction<SkinData> SelectedSkinChanged;

    private SkinDataBase _dataBase;

    public SkinInventory(SkinDataBase dataBase)
    {
        _dataBase = dataBase;
    }

    public void Add(SkinData data)
    {
        _buyedGUID.Add(data.GUID);
    }

    public bool Remove(SkinData data)
    {
        return _buyedGUID.Remove(data.GUID);
    }

    public bool Contains(SkinData data)
    {
        return _buyedGUID.Contains(data.GUID);
    }

    public void SelectSkin(SkinData data)
    {
        _selectedGUID = data.GUID;
        SelectedSkinChanged?.Invoke(data);
    }

    public void DeselectSkin()
    {
        _selectedGUID = string.Empty;
        SelectedSkinChanged?.Invoke(null);
    }

    public void Load()
    {
        if (PlayerPrefs.HasKey(SaveKey) == false)
            return;

        string saveJson = PlayerPrefs.GetString(SaveKey);
        var savedInventory = JsonUtility.FromJson<SkinInventory>(saveJson);

        _buyedGUID = savedInventory._buyedGUID;
        _selectedGUID = savedInventory._selectedGUID;
    }

    public void Save()
    {
        string saveJson = JsonUtility.ToJson(this);
        PlayerPrefs.SetString(SaveKey, saveJson);
        PlayerPrefs.Save();
    }
}
