using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KrakenAccessoryInitializator : MonoBehaviour
{
    [SerializeField] private Transform _container;
    [SerializeField] private AccessoryDataBase _dataBase;

    public KrakenAccessory InstAccessory { get; private set; }

    private void OnEnable()
    {
        AccessoryInventory.SelectedAccessoryChanged += UpdateAccessory;
    }

    private void OnDisable()
    {
        AccessoryInventory.SelectedAccessoryChanged -= UpdateAccessory;
    }

    private void Start()
    {
        var inventory = new AccessoryInventory(_dataBase);
        inventory.Load();

        UpdateAccessory(inventory.SelectedSkin);
    }

    private void UpdateAccessory(AccessoryData accessory)
    {
        if (accessory == null)
        {
            RemoveAccessory();
        }
        else
        {
            RemoveAccessory();
            InstAccessory = Instantiate(accessory.Prefab,_container);
        }
    }

    private void RemoveAccessory()
    {
        if (_container.childCount == 0)
            return;

        for (int i = _container.childCount - 1; i >= 0; i--)
            Destroy(_container.GetChild(i).gameObject);

        InstAccessory = null;
    }
}
