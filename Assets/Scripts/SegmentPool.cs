using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SegmentPool : MonoBehaviour
{
    [SerializeField] private GameObject _segment;
    [SerializeField] private int _poolCount = 25;

    private List<GameObject> _pool;

    private void Awake()
    {
        _pool = new List<GameObject>();
        FillingPool(_poolCount);
    }

    private void FillingPool(int count)
    {
        for(int i = 0; i < count; i++)
        {
            _pool.Add(Instantiate(_segment, transform.position, Quaternion.identity));
        }
    }

    public GameObject GetSegment()
    {
        return _pool[0];
    }

    public void ReturnSegment() => _pool[0].transform.position = transform.position;

}
