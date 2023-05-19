// armin delmo; jigsawpiece.cs; 12/28/22
// the purpose of this program is to follow the player when grabbed, to be put in the slots

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class JigsawPiece : HoldableItem
{
    public int pieceID; // ID to differentiate sections of image
    public int imageID; // ID to differntiate the image

    private void OnTriggerStay2D(Collider2D collision) // run grabbing coroutine on player if they will it
    {
        if(collision.tag == "Player")
        {
            if(collision.GetComponent<PlayerStats>().turnOrder != imageID) // prevent players from holding other ppl's puzzles
            {
                return;
            }
            if(collision.GetComponent<PlayerMovement>().holding == null && collision.GetComponent<PlayerMovement>().acting == true)
            {
                StartCoroutine(collision.GetComponent<PlayerMovement>().HoldRoutine(false, this.gameObject));
            }
            else if(collision.GetComponent<PlayerMovement>().holding == gameObject && collision.GetComponent<PlayerMovement>().acting == true)
            {
                StartCoroutine(collision.GetComponent<PlayerMovement>().HoldRoutine(true, this.gameObject));
            }
        }
    }
}
