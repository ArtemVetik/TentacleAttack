using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class SkinData : GUIDData
{
    [SerializeField] private string _name;
    [SerializeField] private Texture _texture;

    public string Name => _name;
    public Texture Texture => _texture;
}
