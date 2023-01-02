// armin delmo; jigsawpiece.cs; 12/28/22
// the purpose of this program is to follow the player when grabbed, to be put in the slots

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class JigsawPiece : MonoBehaviour
{
    public int pieceID; // ID to differentiate sections of image
    public int imageID; // ID to differntiate the image
    private GameObject plr;
    private bool pick;
    private bool onGoing;
    private void Update() // updating jigsaw position
    {
        if(plr != null)
        {
            this.transform.position = new Vector2(plr.transform.position.x, plr.transform.position.y + .5f); ;
        }
    }
    private void ChangeParent(GameObject target) // piece becomes child of player, follows to be put in jigsaw
    {
        // changing player to target
        plr = target;
    }
    IEnumerator Follow(bool yes, GameObject gO) // see if bool is true; change following
    {
        if (onGoing == false)
        {
            Debug.Log("Running!");
            onGoing = true;
            switch (yes) // check to see if parent should be switched
            {

                case true:
                    ChangeParent(gO);
                    pick = true;

                    gO.GetComponent<PlayerMovement>().holding = true;

                    break;
                case false:
                    plr = null;

                    pick = false;
                    gO.GetComponent<PlayerMovement>().holding = false;
                    break;
            }
            yield return new WaitForSeconds(.5f);
            onGoing = false;
            Debug.Log("Done");
        }
    }
    private void OnTriggerStay2D(Collider2D collision) // check to see if player is acting.
    {
        Debug.Log("Collision Detected");
        if(collision.tag == "Player" && imageID == collision.GetComponent<PlayerControls>().turnOrder) // check to see if player
        {
            Debug.Log("Player identified.");
            if(collision.GetComponent<PlayerMovement>().acting == true) // check to see if acting
            {
                if(collision.GetComponent<PlayerMovement>().holding == false && pick == false)
                {
                    Debug.Log("Attempting to run...");
                    StartCoroutine(Follow(true, collision.gameObject)); // enable
                }
                else if(collision.GetComponent<PlayerMovement>().holding == true)
                {
                    Debug.Log("Attempting to run...");
                    StartCoroutine(Follow(false, collision.gameObject)); // disable
                }

            }
        }
        /*if(collision.tag == "Minigame Element") // checking to see if it is correct piece to put in.
        {
            if(collision.GetComponent<>())
        }
        */
    }
}
