using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AccessoryDataBase", menuName = "Shop/Accessory/AccessoryDataBase", order = 51)]
public class AccessoryDataBase : ScriptableObject
{
    [SerializeField] private int _defaultAccessoryIndex = 0;
    [SerializeField] private List<AccessoryData> _accessories = new List<AccessoryData>();

    public IEnumerable<AccessoryData> Data => _accessories;
    public AccessoryData DefaultData => _accessories[_defaultAccessoryIndex];

    public void Add(AccessoryData data)
    {
        _accessories.Add(data);
    }

    public void RemoveAt(int index)
    {
        if (index >= 0 && index < _accessories.Count)
            _accessories.RemoveAt(index);
    }

    public void MoveFront(int index)
    {
        if (index < 1 || index > _accessories.Count - 1)
            return;

        var temp = _accessories[index];
        _accessories[index] = _accessories[index - 1];
        _accessories[index - 1] = temp;
    }

    public void MoveBack(int index)
    {
        if (index >= _accessories.Count - 1 || index < 0)
            return;

        var temp = _accessories[index];
        _accessories[index] = _accessories[index + 1];
        _accessories[index + 1] = temp;
    }
}
