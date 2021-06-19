using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerContainer : MonoBehaviour
{
    public FooPlayer Player { get; private set; }

    public void Init(FooPlayer activePlayer)
    {
        Player = activePlayer;
    }
}
