// armin delmo; 12/31/22
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JigsawHole : MonoBehaviour
{
    public int imageID; // id of an image
    public int pieceID; // id of a section of an image

    // variables to detect if piece will be taken
    public bool taking = true;

    GameObject holding; // for hole to know

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Minigame Element" && collision.gameObject == holding)
        {
            collision.GetComponent<HoldableItem>().slotted = false;

            // check if the piece was correct.
            if(holding.GetComponent<JigsawPiece>().pieceID == pieceID && taking == false)
            {
                taking = true; // to prevent glitching
                GetComponentInParent<Jigsaw_Board>().ChangeCorrect(-1); // reduce correct pieces by 1
            }
            taking = true;

        }

    }
    private IEnumerator Wait(float time)
    {
        yield return new WaitForSeconds(time);
    }
    private void OnTriggerStay2D(Collider2D collision) // turns on and grabs puzzle pieces
    {
        if(taking == true && collision.tag == "Minigame Element") // beginning statement to prevent error
        {
            Debug.Log("detected.");
            if (collision.GetComponent<HoldableItem>().beingHeld == false && taking == true)
            {
                StartCoroutine(Wait(0.5f));
                if(collision.GetComponent<HoldableItem>().slotted == false && collision.GetComponent<JigsawPiece>().imageID == imageID)
                {
                    Debug.Log("Slotting..");
                    collision.GetComponent<HoldableItem>().slotted = true;
                    taking = false;
                    collision.transform.position = this.gameObject.transform.position;
                    holding = collision.gameObject;

                    // check if the piece is correct.
                    if(holding.GetComponent<JigsawPiece>().pieceID == pieceID)
                    {
                        GetComponentInParent<Jigsaw_Board>().ChangeCorrect(1); // increase correct pieces by 1
                    }
                }
            }
        }        
    }
}
