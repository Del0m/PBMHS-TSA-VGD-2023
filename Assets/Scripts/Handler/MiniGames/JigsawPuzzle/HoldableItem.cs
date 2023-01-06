//armin delmo; 1/3/22
//mother script of all holdable items in minigames, primarily so they can register with the game
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldableItem : MonoBehaviour
{
    public bool beingHeld; // see if in game object being held
    public bool canPickUp; // see if object can be allowed to be grabbed.
    public bool slotted; // prevent other things from taking object
}
