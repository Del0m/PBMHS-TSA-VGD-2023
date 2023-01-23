/*armindelmo turnmanager.cs
 * the purpose of this program is to seperate all the users into their respective turns.
*/
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurnManager : MonoBehaviour
{
    static int currentTurn = 0;
    public int turnsElapsed = 0;


    //manage the minigames

    static MiniGameManager miniGameScript;

    //modify scores of players
    public ScoreManager scoreScript;

    public Sprite[] diceImage;
    // ui to update players on current turn
    [Header("UI")]
    public TextMeshProUGUI roundUI; // tells the round for the players
    public GameObject[] playerUI; // to be highlighted by the game to tell players its their turn!
    public GameObject diceSprite; // to show player how many moves they have left on the board


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

        // run ui update
        RoundUIUpdate(); 
        if(playerCount < currentTurn) // turn on el minigame
        {
            miniGameScript.MinigameStartup(); // running minigame coroutine to advise players, and spawn game.
        }
        
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
    public void RoundUIUpdate() // update the UI on the turn counter in the UI Canvas
    {
        roundUI.text = turnsElapsed.ToString();

        // highlight the current player here [ ]
    }
    public void PlayDiceUI() // plays animation on diceSprite
    {
        diceSprite.GetComponent<Animator>().Play("Dice");
    }
    public void DiceUIUpdate(int dice)
    {
        switch(dice) // switch to change moves left for board via UI
        {
            case 0:
                diceSprite.GetComponent<Image>().sprite = diceImage[0];
                diceSprite.GetComponent<Animator>().playbackTime = 0; // reset dice back to middle
                break;
            case 1:
                diceSprite.GetComponent<Image>().sprite = diceImage[1];
                break;
            case 2:
                diceSprite.GetComponent<Image>().sprite = diceImage[2];
                break;
            case 3:
                diceSprite.GetComponent<Image>().sprite = diceImage[3];
                break;
            case 4:
                diceSprite.GetComponent<Image>().sprite = diceImage[4];
                break;
            case 5:
                diceSprite.GetComponent<Image>().sprite = diceImage[5];
                break;
            case 6:
                diceSprite.GetComponent<Image>().sprite = diceImage[6];
                break;
        }
    }
}
