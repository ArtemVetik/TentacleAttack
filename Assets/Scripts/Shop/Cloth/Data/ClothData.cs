using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class ClothData : GUIDData
{
    [SerializeField] private string _name;
    [SerializeField] private Sprite _preview;
    [SerializeField] private KrakenCloth _prefab;

    public string Name => _name;
    public Sprite Preview => _preview;
    public KrakenCloth Prefab => _prefab;
}
