using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IShopSaved
{
    public abstract string SaveKey { get; }

    void Save();
    void Load();
}
