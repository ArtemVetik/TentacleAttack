using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class AccessoryData : GUIDData
{
    [SerializeField] private string _name;
    [SerializeField] private Sprite _preview;
    [SerializeField] private KrakenAccessory _prefab;

    public string Name => _name;
    public Sprite Preview => _preview;
    public KrakenAccessory Prefab => _prefab;
}
