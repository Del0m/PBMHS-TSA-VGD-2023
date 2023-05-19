/*armindelmo turnmanager.cs
 * the purpose of this program is to seperate all the users into their respective turns.
*/
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurnManager : MonoBehaviour
{
    [Header("Board Variables")]
    public int currentTurn = -1; // where players can't move until the game starts
    public int roundsElapsed;

    public int maxRounds;


    [Header("Scripts")]

    MiniGameManager miniGameScript;

    //Get player manager
    PlayerManager plrManager;

    //check current players space
    MovementManager moveManager;
    [HideInInspector]
    public PlayerUIManager uiManager;
    CameraControl cam;

    [Header("Assigned Objects")]
    public EndGame end; // to run the endgame function
    public GameObject musicObj;

    // ui to update players on current turn
    [Header("UI")]
    public GameObject[] playerUI; // to be highlighted by the game to tell players its their turn!

    private int playerCount;

    private void Awake()
    {
        this.gameObject.tag = "Turn Manager"; //change this object to have turn manager tag
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraControl>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //putting these in if statements because they should be publicly listed and added.
 
        if(moveManager == null)
        {
            moveManager = GameObject.FindGameObjectWithTag("Movement Manager").GetComponent<MovementManager>();

        }
 
        if(miniGameScript == null)
        {
            miniGameScript = GameObject.FindGameObjectWithTag("Mini Game Manager").GetComponent<MiniGameManager>(); // call manager to start / end / bring players to games.
        }

        if(plrManager == null)
        {
            plrManager = GameObject.FindGameObjectWithTag("Player Manager").GetComponent<PlayerManager>();
        }


        //call UI update
        if (!plrManager.singlePlayer)
        {
            uiManager.UpdateRound(roundsElapsed);

        }
    }


    public bool CheckLocation() // see if the player is on their tile
    {
        // checking to see if the player moved to their spot
        var plrPosition = plrManager.player[currentTurn].transform.position;

        var plrTilePosition = plrManager.player[currentTurn].GetComponent<PlayerStats>().position;

        if(Vector2.Distance(plrPosition, moveManager.CallTile(plrTilePosition).position) > 1)
        {
            return false;
        }
        return true;
    }
    public void RoundCheck() // compares currentTurn with player count, exceeds, start minigame
    {
        if(plrManager.player.Count - 1 <= currentTurn) // all players went, game not done? minigame time!
        {
            // resetting the camera
            cam.destination = null;
            //debug
            musicObj.SetActive(false);

            miniGameScript.MinigameStartup(); // running minigame coroutine to advise players, and spawn game.
            
            roundsElapsed++;
        }

        currentTurn++;
        // move camera
        if (currentTurn < plrManager.player.Count)
        {
            musicObj.SetActive(true);
            StartCoroutine(cam.ModifyCamera(plrManager.player[currentTurn].transform, 25, 20, 30));
        }

        // run ui update
        uiManager.UpdateRound(roundsElapsed);
    }
    public bool CheckEnd() // checking if the game is up
    {
        if (roundsElapsed >= maxRounds) // end the game
        {
            StartCoroutine(end.EndRoutine());
            return true;
        }
        return false;
    }
    public void SetTurn(int integer) { currentTurn = integer; } // sets turn back to normal

    public bool RunTurn(GameObject player, int playerTurn) // updates turn for players
    {
        if(playerTurn == currentTurn && cam.AllowMovement()) //check to see if its the players turn
        {
            //move player
            return true;
        }
        return false;
    }
}
