using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ClothDataBase", menuName = "Shop/Cloth/ClothDataBase", order = 51)]
public class ClothDataBase : ScriptableObject
{
    [SerializeField] private int _defaultSnakeIndex = 0;
    [SerializeField] private List<ClothData> _clothes = new List<ClothData>();

    public ClothData DefaultData => _clothes[_defaultSnakeIndex];
    public IEnumerable<ClothData> Data => _clothes;

    public ClothData this[int index]
    {
        get
        {
            return _clothes[index];
        }
    }

    public int IndexOf(ClothData data)
    {
        for (int i = 0; i < _clothes.Count; i++)
            if (_clothes[i].GUID == data.GUID)
                return i;

        return -1;
    }

    public bool TryGetNext(ClothData current, out ClothData next)
    {
        var currentIndex = IndexOf(current);
        if (currentIndex < _clothes.Count - 1)
        {
            next = _clothes[currentIndex + 1];
            return true;
        }
        else
        {
            next = null;
            return false;
        }
    }

    public void Add(ClothData data)
    {
        _clothes.Add(data);
    }

    public void RemoveAt(int index)
    {
        if (index >= 0 && index < _clothes.Count)
            _clothes.RemoveAt(index);
    }

    public void MoveFront(int index)
    {
        if (index < 1 || index > _clothes.Count - 1)
            return;

        var temp = _clothes[index];
        _clothes[index] = _clothes[index - 1];
        _clothes[index - 1] = temp;
    }

    public void MoveBack(int index)
    {
        if (index >= _clothes.Count - 1 || index < 0)
            return;

        var temp = _clothes[index];
        _clothes[index] = _clothes[index + 1];
        _clothes[index + 1] = temp;
    }
}
