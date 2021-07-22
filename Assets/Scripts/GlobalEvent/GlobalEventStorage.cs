using System;
using UnityEngine;

public static class GlobalEventStorage
{
    public static event Action<TentacleSegment> TentacleAddDamage;
    public static event Action<bool> GameOvering;
    public static event Action<bool> GameEnded;


    #region TentacleAddDamage
    public static void TentacleAddDamageInvoke(TentacleSegment segment)
    {
        TentacleAddDamage?.Invoke(segment);
    }
    #endregion

    #region TentacleDied

    public static void GameOveringInvoke(bool isWin)
    {
        GameOvering?.Invoke(isWin);
    }
    #endregion

    #region EndGame

    public static void GameEndedInvoke(bool isWin)
    {
        GameEnded?.Invoke(isWin);
    }

    #endregion
}

