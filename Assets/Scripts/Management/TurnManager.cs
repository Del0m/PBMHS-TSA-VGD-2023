/*armindelmo turnmanager.cs
 * the purpose of this program is to seperate all the users into their respective turns.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class TurnManager : MonoBehaviour
{
    private PlayerManager playerManager;
    public GameObject[] players;

    public int currentTurn = 1;
    public int turnsElapsed = 0;
    int previousTurn = 1; // for the purposes of checking if currentturn has updated!
    static string winner;

    //minigame Management elements

    public MiniGameManager miniGameScript;

    //grab score manager
    private ScoreManager scoreScript;

    //ui elements
    private GameObject movementUI;
    private GameObject turnUI;

    public TextMeshProUGUI winnerText;
    public GameObject winnerScreen;

    private void Awake()
    {
        this.gameObject.tag = "Turn Manager";//change this object to have turn manager tag
        winnerScreen.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        scoreScript = GameObject.FindGameObjectWithTag("Score Manager").GetComponent<ScoreManager>();
        playerManager = GameObject.FindGameObjectWithTag("Player Manager").GetComponent<PlayerManager>(); // call player manager for player array
        miniGameScript = GameObject.FindGameObjectWithTag("Mini Game Manager").GetComponent<MiniGameManager>(); // call manager to start / end / bring players to games.


        //processing ui elements
        movementUI = GameObject.Find("Movement Turn");
    }
    private void Update()
    {
        NewRound();
        UpdateTurn();
    }
    public void ChangeTurn() // to be called from player controls to change to the next turn
    {
        currentTurn = currentTurn + 1; // update to next turn
        turnUI.GetComponent<TextMeshProUGUI>().text = "Player " + (currentTurn+1).ToString() + "'s turn!";

    }
    public void UpdateTurn()
    {
        if(previousTurn != currentTurn)
        {
            for (int i = 0; i < players.Length; i++) // runPlayerTurn on all players
            {
                players[i].GetComponent<PlayerControls>().PlayerTurn(); // update turns on player array.
                Debug.Log("Updating Player " + i);
            }

            previousTurn = currentTurn;
        }
        
    }
    public void NewRound() // if current turn > player.length; run minigame
    {
        if (turnsElapsed > 9) // end the game
        {
            Debug.Log("Ending the game!");
            var highestScore = 0;
            var highestPlayer = 0;
            for (int i = 0; i < scoreScript.playerS.Length; i++) // check to see who has the highest score
            {
                if (highestScore < scoreScript.playerS[i].Score)
                {
                    highestPlayer = i;
                }
                winner = i.ToString();

            }
            winnerScreen.SetActive(true);
            string PlayerWinner = "Player" + (highestPlayer + 1).ToString() + " has Won!";
            winnerText.text = PlayerWinner;
            StartCoroutine(waitToLoad());
        }
        while (currentTurn > players.Length)
        {
#if UNITY_EDITOR
            if(players.Length == 0)
            {
                break;
            }
#endif
            
            for (int i = 0; i < players.Length; i++)
            {
                //check all player turns
                players[i].GetComponent<PlayerControls>().PlayerTurn();
            }
            if (miniGameScript.hasStarted == false)
            {
                currentTurn = 1;
                miniGameScript.StartMiniGame();
                miniGameScript.hasStarted = true;
            }
        }
        
    }

    IEnumerator waitToLoad()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("EndScene");
    }

    public void GetPlayers(GameObject[] playerArray) // grab players from PlayerManager and put into array
    {
        players = playerArray; // grab array given from player Manager
        miniGameScript.GetPlayers(playerArray);
    }
    // on second thought, this is a very secure solution... Wont be used tho :-)
}
