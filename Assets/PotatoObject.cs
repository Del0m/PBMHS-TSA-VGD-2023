using System.Collections;
using System.Security.Cryptography;
using UnityEditorInternal;
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

    [Header("Highlight Objects")]
    // game object to highlight who it will be thrown to; hold onto object to delete later
    public GameObject highlight; // player highlighted
    public GameObject highlightObject; // highlight under player
    public GameObject highlightInstance; // instantiation as a variable to later delete

    public SpriteRenderer sprite; // sprite called to change the colors
    void Update()
    {
        Explode();
        MovePotato();

        StartCoroutine(Pass());
        ChangeHighlight();

        ChangeScale(); // visual aid

        if(highlightObject == null)
        {
            highlightObject = Resources.Load("Prefabs/Mini-Games/Minigame Assets/Highlight", typeof(GameObject)) as GameObject;
            Resources.UnloadUnusedAssets(); // prevent memory leak
        }
    }
    float timeElapsed; // to scale the potato object as the time goes on.
    bool explodeAnim; // to enable the explosion animation

    void ChangeScale() // this function is to change the scale of the object to animate it.
    {
        timeElapsed += Time.deltaTime;
        var scaleModifier = 1 + (timeElapsed / 10);
        if(this.gameObject.transform.localScale.x <= 1) // check to see if it hasn't increased
        {
            this.gameObject.transform.localScale = new Vector3
                (scaleModifier,
                scaleModifier,
                scaleModifier);

            // change the color of the sprite closer to red to indicate explosion!
            sprite.color = new Color(255, sprite.color.g - .01f, sprite.color.b - .01f);

        }
        else if (explodeAnim == false) // slowly scale down to come back up
        {
            this.gameObject.transform.localScale = new Vector3
                (this.gameObject.transform.localScale.x - Time.deltaTime, 
                this.gameObject.transform.localScale.y - Time.deltaTime,
                1f);

        }
        if(explodeAnim == true)
        {
            this.gameObject.transform.localScale = new Vector3
        (this.gameObject.transform.localScale.x + Time.deltaTime,
        this.gameObject.transform.localScale.y + Time.deltaTime,
        1f);
        }
    }
    bool hasRan;
    void Explode() // check if time is up and destroy itself and penalize player
    {

        if (timeLeft < 0 && hasRan == false) // time is up
        {
            hasRan = true;
            Debug.Log("Exploded.");
            StartCoroutine(parentScript.EndGame(holdingPlayer, true));

            Destroy(highlightInstance); // destroy the instance of highlight

            Destroy(this.gameObject, 1.5f); // destroy game object as it isn't needed
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
                if (potentialPass == holdingPlayer)
                {
                    potentialPass++;
                }
                highlight = parentScript.player[potentialPass];

            }
            else
            {
                if (potentialPass == holdingPlayer)
                {
                    potentialPass++;
                }
                highlight = parentScript.player[potentialPass];
            }
        }
        else if(increment == -1 && routineOngoing == false)
        {
            routineOngoing = true;
            potentialPass += increment; // move pass to the left

            if (holdingPlayer + potentialPass < 0)
            {
                potentialPass = parentScript.player.Length - 1;
                if (potentialPass == holdingPlayer)
                {
                    potentialPass--;
                }
                highlight = parentScript.player[potentialPass]; // move to last player in array
            }
            else
            {
                if (potentialPass == holdingPlayer)
                {
                    potentialPass--;
                }
                if(holdingPlayer < 0) { potentialPass = parentScript.player.Length - 1; } // prevent negative numbers
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
        if (highlight != null) { HighlightPass(); } // prevent error spam
    }
    void HighlightPass() // show player who they will be passing to
    {
        if(highlightInstance) // check if it exists before potentially spitting error out
        {
            Destroy(highlightInstance);
            highlightInstance = null;
        }
        // highlight the player that can be thrown to.
        highlightInstance = Instantiate(highlightObject, highlight.transform.position,  new Quaternion(0,0,0,0));

    }
    IEnumerator Pass() // see if conditions to pass from one player to another are met, and then pass to player  
    {
        // if holding player acts to throw object.
        if (parentScript.player[holdingPlayer].GetComponent<PlayerMovement>().acting == true)
        {
            Debug.Log("Passing!");
            try
            {
                holdingPlayer = potentialPass; // change integer to now be the player who is holding the potato

                if (parentScript.player[holdingPlayer] == null) { throw new System.Exception(); }
                var failSafe = parentScript.player[holdingPlayer]; // to check if code fails (wrong player selected)
                Destroy(highlightInstance); // delete highlight to prevent it from staying onto player
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
