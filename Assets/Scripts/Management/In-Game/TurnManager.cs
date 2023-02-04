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


    //manage the minigames

    public MiniGameManager miniGameScript;

    //Get player manager
    public PlayerManager pm;

    //modify scores of players
    public ScoreManager scoreScript;

    public PlayerUIManager uiManager;

    public CameraControl cam;

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
 /*
        if(scoreScript == null)
        {
            scoreScript = GameObject.FindGameObjectWithTag("Score Manager").GetComponent<ScoreManager>();

        }
 */
        if(miniGameScript == null)
        {
            miniGameScript = GameObject.FindGameObjectWithTag("Mini Game Manager").GetComponent<MiniGameManager>(); // call manager to start / end / bring players to games.
        }

        if(pm == null)
        {
            pm = GameObject.FindGameObjectWithTag("Player Manager").GetComponent<PlayerManager>();
            //Get player list length
            StartCoroutine(getPlayers());
        }

        //call UI update
        uiManager.UpdateRound(roundsElapsed);

    }

    IEnumerator getPlayers()
    {
        yield return new WaitForSeconds(5);
        playerCount = pm.player.Length;
        StopCoroutine(getPlayers());
    }

    public void RoundCheck() // compares currentTurn with player count, exceeds, start minigame
    {
        // [] add a highlight player during turn here...

        Debug.Log("Checking new round! Advancing Turn!");
        currentTurn++;

        // move camera
        if(currentTurn <= pm.player.Length)
        {
            StartCoroutine(cam.ModifyCamera(pm.player[currentTurn].transform, 25, 20, 30));

        }
        // run ui update
        uiManager.UpdateRound(roundsElapsed);
        if(playerCount <= currentTurn) // turn on el minigame
        {
            //debug
            Debug.Log("starting the minigame");

            miniGameScript.MinigameStartup(); // running minigame coroutine to advise players, and spawn game.

            //currentTurn = 0;

            roundsElapsed++;
        }

        //currentTurn++;
        
    }
    public void SetTurn(int integer) { currentTurn = integer; } // sets turn back to normal

    public bool RunTurn(GameObject player, int playerTurn) // updates turn for players
    {
        if(playerTurn == currentTurn) //check to see if its the players turn
        {
            //move player
            Debug.Log("Running turn");
            return true;
        }
        Debug.Log("Turn failed, " + currentTurn + " does not equal " + playerTurn);
        return false;
    }
}
