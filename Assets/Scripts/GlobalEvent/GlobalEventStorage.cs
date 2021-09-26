using System;
using UnityEngine;

public static class GlobalEventStorage
{
    public static event Action<TentacleSegment> TentacleAddDamage;
    public static event Action<bool> GameOvering;
    /// <summary>
    /// Invoke after tentacle full rewinding
    /// </summary>
    public static event Action<bool, int> GameEnded;


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

    public static void GameEndedInvoke(bool isWin, int progress)
    {
        GameEnded?.Invoke(isWin, progress);
    }

    #endregion
}

