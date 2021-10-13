using System;
using UnityEngine;

public static class GlobalEventStorage
{
    public static event Action<TentacleSegment> TentacleAddDamage;
    public static event Action<bool> GameOvering;
    public static event Action EnemyEnded;
    /// <summary>
    /// Invoke after tentacle full rewinding
    /// </summary>
    public static event Action<bool, int> GameEnded;


    public static void TentacleAddDamageInvoke(TentacleSegment segment)
    {
        TentacleAddDamage?.Invoke(segment);
    }

    public static void GameOveringInvoke(bool isWin)
    {
        GameOvering?.Invoke(isWin);
    }

    public static void EnemyEndedInvoke()
    {
        EnemyEnded?.Invoke();
    }

    public static void GameEndedInvoke(bool isWin, int progress)
    {
        GameEnded?.Invoke(isWin, progress);
    }
}

