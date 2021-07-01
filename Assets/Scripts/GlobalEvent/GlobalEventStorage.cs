using System;
using UnityEngine;

public static class GlobalEventStorage
{
    private static event Action TentacleAddDamage;

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
}

