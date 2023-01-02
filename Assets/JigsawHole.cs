// armin delmo; 12/31/22
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JigsawHole : MonoBehaviour
{
    public int imageID; // id of an image
    public int pieceID; // id of a section of an image

    // variables to detect if piece will be taken
    public bool taking;
    public bool inProgress; // to prevent multiple runs
    IEnumerator EnableTake(float time) // hold something open for a select amount of time
    {
        if(inProgress == false)
        {
            Debug.Log("Running EnableTake()");
            inProgress = true;

            taking = true; // allows puzzle pieces to be taken
            yield return new WaitForSeconds(time);
            taking = false; // disallows puzzle pieces to be taken
            inProgress = false;
        }
    }
    private void OnTriggerStay2D(Collider2D collision) // turns on and grabs puzzle pieces
    {
        Debug.Log("Triggered");
        if (collision.tag == "Minigame Element") // to see if it is a puzzle piece
        {
            Debug.Log("Puzzle piece detected...");
            if (taking == true)
            {
                Debug.Log("Moving the piece");
                collision.transform.position = this.gameObject.transform.position; // set puzzle piece in.
                taking = false;
            }
        }

        if (collision.tag == "Player")
        {
            Debug.Log("Player Triggered");
            if (collision.GetComponent<PlayerMovement>().holding == true)
            {
                taking = false;
            }
            if(collision.GetComponent<PlayerMovement>().holding == false && collision.GetComponent<PlayerMovement>().acting == true)
            {
                taking = true;
            }
        }
    
        
    }
}
