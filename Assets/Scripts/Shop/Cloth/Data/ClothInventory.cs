using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class ClothInventory : IShopSaved
{
    [SerializeField] private List<string> _buyedGUID = new List<string>();
    [SerializeField] private List<string> _availableGUID = new List<string>();
    [SerializeField] private string _selectedGUID;

    public string SaveKey => "ClothInventorySaveKey";

    public ClothData Last => _dataBase.Data.First(data => data.GUID == _buyedGUID.Last());
    public ClothData SelectedSkin
    {
        get
        {
            if (string.IsNullOrEmpty(_selectedGUID))
                return null;

            return _dataBase.Data.First((data) => data.GUID == _selectedGUID);
        }
    }
    public IEnumerable<ClothData> Data => from data in _dataBase.Data
                                              where _buyedGUID.Contains(data.GUID)
                                              select data;

    private ClothDataBase _dataBase;

    public ClothInventory(ClothDataBase dataBase)
    {
        _dataBase = dataBase;
        if (string.IsNullOrEmpty(_selectedGUID))
        {
            Add(_dataBase.DefaultData);
            SelectCloth(_dataBase.DefaultData);
        }
    }

    public void Add(ClothData data)
    {
        _buyedGUID.Add(data.GUID);
    }

    public void AddAvailable(ClothData data)
    {
        _availableGUID.Add(data.GUID);
    }

    public bool Remove(ClothData data)
    {
        return _buyedGUID.Remove(data.GUID);
    }

    public bool ContainsBuyed(ClothData data)
    {
        return _buyedGUID.Contains(data.GUID);
    }

    public bool ContainsAvaiable(ClothData data)
    {
        return _availableGUID.Contains(data.GUID);
    }

    public void SelectCloth(ClothData data)
    {
        _selectedGUID = data.GUID;
    }

    public void DeselectAccessory()
    {
        _selectedGUID = string.Empty;
    }

    public void Load()
    {
        if (PlayerPrefs.HasKey(SaveKey) == false)
            return;

        var saveJson = PlayerPrefs.GetString(SaveKey);
        var savedInventory = JsonUtility.FromJson<ClothInventory>(saveJson);

        _buyedGUID = savedInventory._buyedGUID;
        _availableGUID = savedInventory._availableGUID;
        _selectedGUID = savedInventory._selectedGUID;
    }

    public void Save()
    {
        string saveJson = JsonUtility.ToJson(this);
        PlayerPrefs.SetString(SaveKey, saveJson);
        PlayerPrefs.Save();
    }
}
