//armin delmo, this game is a bunch of cards that get matched with each other. First person to finish wins.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; // for sensing input
using TMPro; // for text mesh modification
public class MemoryMatch : GameHandler
{
    public override void JumpStart()
    {
        base.JumpStart();
        //build cards
        BuildCards();
    }
}
