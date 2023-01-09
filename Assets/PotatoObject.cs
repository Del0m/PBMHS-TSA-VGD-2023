using System.Collections;
using UnityEngine;

public class PotatoObject : MonoBehaviour
{
    // variables that are brought from minigame script
    public HotPotato parentScript; // to grab ScorePlayer function
    public double timeLeft; // time till explosion!

    // the player that is holding the potato
    public int holdingPlayer;
    // who will be passed the potato
    public int potentialPass; // can only be 0,1 left, right

    // game object to highlight who it will be thrown to; hold onto object to delete later
    public GameObject highlight;
    public GameObject highlightInstance;

    // booleans to allow bomb to move from one player to another
    bool canMove;
    void Update()
    {
        timeLeft -= Time.deltaTime; // descend time until 0, at which function will make it explode with the player
        Explode();
        MovePotato();
    }
    void Explode() // check if time is up and destroy itself and penalize player
    {
        if (timeLeft < 0) // time is up
        {
            parentScript.ScorePlayer(holdingPlayer, -1); // penalize player for dying with the potato
            parentScript.EndGame();
        }
    }
    void MovePotato() // move the potato from one player to another
    {
        if(canMove == true)
        {
            // moves potato to new player highlighted at 15 units a second
            Vector2.MoveTowards(this.gameObject.transform.position, highlightInstance.transform.parent.position, 15*Time.deltaTime);
        }
    }
    void HighlightPass() // show player who they will be passing to
    {
        if(highlightInstance) // check if it exists before potentially spitting error out
        {
            Destroy(highlightInstance);
        }
        // highlight the player that can be thrown to.
        highlightInstance = Instantiate(highlight, parentScript.player[holdingPlayer + potentialPass].transform);

    }
    IEnumerator Pass() // see if conditions to pass from one player to another are met, and then pass to player  
    {
        // if holding player acts to throw object.
        if (parentScript.player[holdingPlayer].GetComponent<PlayerMovement>().acting == true)
        {
            try
            {
                holdingPlayer += potentialPass; // change integer to now be the player who is holding the potato
                
            }
            catch (System.Exception)
            {
                Debug.LogError("Invalid pass made! Check code for errors.");
                throw;
            }

            canMove = true;
            yield return new WaitForSeconds(0.5f); // wait half a second before disallowing movement
            canMove = false;

        }
    }

}
