using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "ClothRewardList", menuName = "Shop/Cloth/LevelRewardList")]
public class ClothRewardList : ScriptableObject
{
    [SerializeField] private List<ClothRewardData> _levelRewardData = new List<ClothRewardData>();

    public IEnumerable<ClothRewardData> Data => _levelRewardData;

    public bool ContainsLevel(int buildIndex)
    {
        foreach (var data in _levelRewardData)
            if (data.LevelBuildIndex == buildIndex)
                return true;

        return false;
    }

    public bool TryGetByLevel(int levelBuildIndex, out ClothRewardData rewardData)
    {
        foreach (var data in _levelRewardData)
        {
            if (data.LevelBuildIndex == levelBuildIndex)
            {
                rewardData = data;
                return true;
            }
        }

        rewardData = null;
        return false;
    }
}

[Serializable]
public class ClothRewardData
{
    [SerializeField] private int _levelBuildIndex;
    [SerializeField] private int _clothDataBaseIndex;

    public int LevelBuildIndex => _levelBuildIndex;
    public int ClothDataBaseIndex => _clothDataBaseIndex;
}
