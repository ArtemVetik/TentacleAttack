using System;
using UnityEngine;

public static class GlobalEventStorage
{
    private static event Action TentacleAddDamage;
    private static event Action TentacleDied;

    #region TentacleAddDamage
    public static void TentacleAddDamageAddListener(Action method)
    {
        TentacleAddDamage += method;
    }

    public static void TentacleAddDamageRemoveListener(Action method)
    {
        TentacleAddDamage -= method;
    }

    public static void TentacleAddDamageInvoke()
    {
        TentacleAddDamage?.Invoke();
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
}

