using System.Collections;
using System.Security.Cryptography;
using UnityEngine;

public class PotatoObject : MonoBehaviour
{
    // variables that are brought from minigame script
    public HotPotato parentScript; // to grab ScorePlayer function
    public double timeLeft; // time till explosion!

    // the player that is holding the potato
    public int holdingPlayer;
    // who will be passed the potato
    public int potentialPass; // integer to select player in array it will be thrown to

    // game object to highlight who it will be thrown to; hold onto object to delete later
    public GameObject highlight; // player highlighted
    public GameObject highlightObject; // highlight under player
    public GameObject highlightInstance; // instantiation as a variable to later delete


    void Update()
    {
        Explode();
        MovePotato();

        StartCoroutine(Pass());
        ChangeHighlight();

        if(highlightObject == null)
        {
            highlightObject = Resources.Load("Prefabs/Mini-Games/Minigame Assets/Highlight", typeof(GameObject)) as GameObject;
            Resources.UnloadUnusedAssets(); // prevent memory leak
        }
    }
    bool hasRan;
    void Explode() // check if time is up and destroy itself and penalize player
    {

        if (timeLeft < 0 && hasRan == false) // time is up
        {
            hasRan = true;
            Debug.Log("Exploded.");
            parentScript.ScorePlayer(holdingPlayer, -1); // penalize player for dying with the potato
            StartCoroutine(parentScript.EndGame());

            Destroy(this.gameObject, 1f); // destroy game object as it isn't needed
        }
        else
        {
            timeLeft -= Time.deltaTime; // descend time until 0, at which function will make it explode with the player
        }
    }
    void MovePotato() // move the potato from one player to another
    {
        // moves potato to new player highlighted at 15 units a second

        transform.position = Vector2.MoveTowards(this.transform.position, parentScript.player[holdingPlayer].transform.position, 15 * Time.deltaTime);
    }
    bool routineOngoing;
    IEnumerator HighlightRoutine(int increment)
    {
        if(increment == 1 && routineOngoing == false)
        {
            routineOngoing = true;
            // increment is what it will increase in said direction, overflowFallback is to set array back into bounds
            potentialPass += increment; // move pass to the right
            if (potentialPass == holdingPlayer)
            {
                potentialPass += increment; // to prevent highlighting self
                Debug.Log("Currently highlighting self, attempting to fix.");
            }
            if (parentScript.player.Length < holdingPlayer + potentialPass)
            {
                potentialPass = 0; // reset back to first player
                highlight = parentScript.player[potentialPass];
            }
            else
            {
                highlight = parentScript.player[potentialPass];
            }
        }
        else if(increment == -1 && routineOngoing == false)
        {
            routineOngoing = true;
            potentialPass += increment; // move pass to the left
            if(potentialPass == holdingPlayer)
            {
                potentialPass += increment; // to prevent highlighting self
                Debug.Log("Currently highlighting self, attempting to fix.");
            }
            if (holdingPlayer + potentialPass < 0)
            {
                potentialPass = parentScript.player.Length - 1;
                if (potentialPass == holdingPlayer)
                highlight = parentScript.player[potentialPass]; // move to last player in array
            }
            else
            {
                highlight = parentScript.player[potentialPass];
            }
        }
        yield return new WaitForSeconds(0.25f);
        routineOngoing = false;
    }
    void ChangeHighlight() // to move the highlight from one player to another
    {
        if (parentScript.player[holdingPlayer].GetComponent<PlayerMovement>().ReadMoveValues('x') > 0) // move right
        {
            StartCoroutine(HighlightRoutine(1));
        }
        else if(parentScript.player[holdingPlayer].GetComponent<PlayerMovement>().ReadMoveValues('x') < 0) // move left
        {
            StartCoroutine(HighlightRoutine(-1));
        }
        HighlightPass();
    }
    void HighlightPass() // show player who they will be passing to
    {
        if(highlightInstance) // check if it exists before potentially spitting error out
        {
            Destroy(highlightInstance);
            highlightInstance = null;
        }
        // highlight the player that can be thrown to.
        highlightInstance = Instantiate(highlightObject, highlight.transform);

    }
    IEnumerator Pass() // see if conditions to pass from one player to another are met, and then pass to player  
    {
        // if holding player acts to throw object.
        if (parentScript.player[holdingPlayer].GetComponent<PlayerMovement>().acting == true)
        {
            try
            {
                holdingPlayer = potentialPass; // change integer to now be the player who is holding the potato
                var failSafe = parentScript.player[holdingPlayer]; // to check if code fails (wrong player selected)
            }
            catch (System.Exception)
            {
                Debug.LogError("Invalid pass made! Check code for errors.");
                throw;
            }
            yield return null;

        }
    }

}
