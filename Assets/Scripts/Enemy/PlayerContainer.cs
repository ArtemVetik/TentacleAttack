using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerContainer : MonoBehaviour
{
    public TentacleSegment Player { get; private set; }

    public void Init(TentacleSegment activePlayer)
    {
        Player = activePlayer;
    }
}
