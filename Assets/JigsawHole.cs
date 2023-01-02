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

    GameObject holding; // for the triggers
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            holding = collision.GetComponent<PlayerMovement>().holding;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Minigame Element")
        {
            taking = true;
        }
    }
    private void OnTriggerStay2D(Collider2D collision) // turns on and grabs puzzle pieces
    {
        if (collision.tag == "Player")
        {
            if (holding != null && holding.tag == "Minigame Element") // check to see if holding minigame item
            {
                Debug.Log("detected.");
                if(collision.GetComponent<PlayerMovement>().acting == true && taking == true)
                {
                    holding.transform.position = this.gameObject.transform.position;
                    taking = false;
                }
            }
        }
    
        
    }
}
