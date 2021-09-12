using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPanel : MonoBehaviour
{
    [SerializeField] private SkinDataBase _skinDataBase;

    public void AddCoins(int value)
    {
        SaveDataBase.SetScore(SaveDataBase.GetScore() + value);
    }

    public void RemoveCoins(int value)
    {
        var score = SaveDataBase.GetScore();
        if (score < value)
            return;

        SaveDataBase.SetScore(score - value);
    }

    public void ResetSkins()
    {
        var inventory = new SkinInventory(_skinDataBase);
        inventory.Save();
    }
}
