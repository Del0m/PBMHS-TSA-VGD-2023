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
    public GameObject[] players;

    public int currentTurn = 1;
    public int turnsElapsed = 0;


    //minigame Management elements

    public MiniGameManager miniGameScript;

    //grab score manager
    private ScoreManager scoreScript;


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
        miniGameScript = GameObject.FindGameObjectWithTag("Mini Game Manager").GetComponent<MiniGameManager>(); // call manager to start / end / bring players to games.


    }
    private void Update()
    {

    }
    public void RunTurn(GameObject player) // updates turn for players
    {
        var playerTurn = player.GetComponent<PlayerControls>().turnOrder;
        if(playerTurn == currentTurn) //check to see if its the players turn
        {
            //move player
            player.GetComponent<PlayerControls>().Moving(2); // 2 is the wait time between moves
        }
    }

}
