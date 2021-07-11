using System;
using UnityEngine;

public static class GlobalEventStorage
{
    private static event Action<TentacleSegment> TentacleAddDamage;
    private static event Action TentacleDied;
    private static event Action<bool> GameEnding;

    #region TentacleAddDamage
    public static void TentacleAddDamageAddListener(Action<TentacleSegment> method)
    {
        TentacleAddDamage += method;
    }

    public static void TentacleAddDamageRemoveListener(Action<TentacleSegment> method)
    {
        TentacleAddDamage -= method;
    }

    public static void TentacleAddDamageInvoke(TentacleSegment segment)
    {
        TentacleAddDamage?.Invoke(segment);
    }
    #endregion

    #region TentacleDied
    public static void TentacleDiedAddListener(Action method)
    {
        TentacleDied += method;
    }

    public static void TentacleDiedRemoveListener(Action method)
    {
        TentacleDied -= method;
    }

    public static void TentacleDiedInvoke()
    {
        TentacleDied?.Invoke();
    }
    #endregion

    #region EndGame

    public static void GameEndedAddListener(Action<bool> method)
    {
        GameEnding += method;
    }

    public static void GameEndedRemoveListener(Action<bool> method)
    {
        GameEnding -= method;
    }

    public static void GameEndedInvoke(bool isWin)
    {
        GameEnding?.Invoke(isWin);
    }

    #endregion
}

