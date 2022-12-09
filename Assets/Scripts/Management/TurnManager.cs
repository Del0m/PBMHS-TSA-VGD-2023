/*armindelmo turnmanager.cs
 * the purpose of this program is to seperate all the users into their respective turns.
*/
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    static int currentTurn = 0;
    public int turnsElapsed = 0;


    //manage the minigames

    static MiniGameManager miniGameScript;

    //modify scores of players
    public ScoreManager scoreScript;


    private void Awake()
    {
        this.gameObject.tag = "Turn Manager";//change this object to have turn manager tag

    }

    // Start is called before the first frame update
    void Start()
    {
        //putting these in if statements because they should be publicly listed and added.
 
        if(scoreScript == null)
        {
            scoreScript = GameObject.FindGameObjectWithTag("Score Manager").GetComponent<ScoreManager>();

        }
        if(miniGameScript == null)
        {
            miniGameScript = GameObject.FindGameObjectWithTag("Mini Game Manager").GetComponent<MiniGameManager>(); // call manager to start / end / bring players to games.
        }


    }
    public void RoundCheck() // compares currentTurn with player count, exceeds, start minigame
    {
        Debug.Log("Checking new round! Advancing Turn!");
        var playerCount = GameObject.FindGameObjectsWithTag("Player").Length;
        currentTurn++;
        if(playerCount < currentTurn) // turn on el minigame
        {
            Debug.Log(miniGameScript);
            miniGameScript.MinigameStartup(); // running minigame coroutine to advise players, and spawn game.
        }
        
    }
    public void SetTurn(int integer) { currentTurn = integer; } // sets turn back to normal

    public bool RunTurn(GameObject player) // updates turn for players
    {
        var playerTurn = player.GetComponent<PlayerControls>().turnOrder;
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
