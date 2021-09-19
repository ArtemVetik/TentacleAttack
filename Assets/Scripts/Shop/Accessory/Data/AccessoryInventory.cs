using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class AccessoryInventory : IShopSaved
{
    [SerializeField] private List<string> _buyedGUID = new List<string>();
    [SerializeField] private string _selectedGUID;

    public string SaveKey => "AccessoryInventorySaveKey";
    public AccessoryData SelectedSkin
    {
        get
        {
            if (string.IsNullOrEmpty(_selectedGUID))
                return null;

            return _dataBase.Data.First((data) => data.GUID == _selectedGUID);
        }
    }
    public IEnumerable<AccessoryData> Data => from data in _dataBase.Data
                                         where _buyedGUID.Contains(data.GUID)
                                         select data;

    public static event UnityAction<AccessoryData> SelectedAccessoryChanged;

    private AccessoryDataBase _dataBase;

    public AccessoryInventory(AccessoryDataBase dataBase)
    {
        _dataBase = dataBase;
    }

    public void Add(AccessoryData data)
    {
        _buyedGUID.Add(data.GUID);
    }

    public bool Remove(AccessoryData data)
    {
        return _buyedGUID.Remove(data.GUID);
    }

    public bool Contains(AccessoryData data)
    {
        return _buyedGUID.Contains(data.GUID);
    }

    public void SelectAccessory(AccessoryData data)
    {
        _selectedGUID = data.GUID;
        SelectedAccessoryChanged?.Invoke(data);
    }

    public void DeselectAccessory()
    {
        _selectedGUID = string.Empty;
        SelectedAccessoryChanged?.Invoke(null);
    }

    public void Load()
    {
        if (PlayerPrefs.HasKey(SaveKey) == false)
            return;

        var saveJson = PlayerPrefs.GetString(SaveKey);
        var savedInventory = JsonUtility.FromJson<AccessoryInventory>(saveJson);

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
