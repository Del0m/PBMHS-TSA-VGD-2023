/*armindelmo turnmanager.cs
 * the purpose of this program is to seperate all the users into their respective turns.
*/
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurnManager : MonoBehaviour
{
    public int currentTurn = 0;
    public int roundsElapsed = 0;

    public int maxRounds = 6;


    //manage the minigames

    public MiniGameManager miniGameScript;

    //Get player manager
    public PlayerManager plrManager;

    //public EndGame endGame;

    //check current players space
    public MovementManager moveManager;

    public PlayerUIManager uiManager;

    public CameraControl cam;

    public GameObject musicObj;

    // ui to update players on current turn
    [Header("UI")]
    public TextMeshProUGUI roundUI; // tells the round for the players
    public GameObject[] playerUI; // to be highlighted by the game to tell players its their turn!

    private int playerCount;

    private void Awake()
    {
        this.gameObject.tag = "Turn Manager";//change this object to have turn manager tag
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

        // run ui update
        uiManager.UpdateRound(roundsElapsed);
        if(playerCount < currentTurn && roundsElapsed != maxRounds) // turn on el minigame
        {
            // resetting the camera
            cam.destination = null;
            //debug
            musicObj.SetActive(false);

            miniGameScript.MinigameStartup(); // running minigame coroutine to advise players, and spawn game.

            //currentTurn = 0;

            roundsElapsed++;
        }

        if(roundsElapsed >= maxRounds)
        {
            //End Game
            // havent wrote anything in here, make another EndGame() function
        }

        currentTurn++;
        // move camera
        if (currentTurn < plrManager.player.Count)
        {
            musicObj.SetActive(true);
            StartCoroutine(cam.ModifyCamera(plrManager.player[currentTurn].transform, 25, 20, 30));
        }
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
    public IEnumerator Callback() // calls the roundcheck again to see if the player has gotten into their spot, to advance the turn
    {
        yield return new WaitForSeconds(1f);
        RoundCheck();
    }
}
