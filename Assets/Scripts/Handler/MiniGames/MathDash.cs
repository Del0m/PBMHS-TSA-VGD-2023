//armin delmo; Mathdash.cs; this is the math dash minigame

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathDash : GameHandler
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(TeleportPlayers()); // teleport players to the minigame.
    }
}
