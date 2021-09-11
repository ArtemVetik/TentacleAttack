using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkinDataBase", menuName = "Shop/Skin/SkinDataBase", order = 51)]
public class SkinDataBase : ScriptableObject
{
    [SerializeField] private int _defaultSkinIndex = 0;
    [SerializeField] private List<SkinData> _skins = new List<SkinData>();

    public IEnumerable<SkinData> Data => _skins;
    public SkinData DefaultData => _skins[_defaultSkinIndex];

    public void Add(SkinData data)
    {
        _skins.Add(data);
    }

    public void RemoveAt(int index)
    {
        if (index >= 0 && index < _skins.Count)
            _skins.RemoveAt(index);
    }

    public void MoveFront(int index)
    {
        if (index < 1 || index > _skins.Count - 1)
            return;

        var temp = _skins[index];
        _skins[index] = _skins[index - 1];
        _skins[index - 1] = temp;
    }

    public void MoveBack(int index)
    {
        if (index >= _skins.Count - 1 || index < 0)
            return;

        var temp = _skins[index];
        _skins[index] = _skins[index + 1];
        _skins[index + 1] = temp;
    }
}
