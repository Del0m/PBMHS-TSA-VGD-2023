/*armindelmo turnmanager.cs
 * the purpose of this program is to seperate all the users into their respective turns.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class TurnManager : MonoBehaviour
{
    public int currentTurn = 1;
    public int turnsElapsed = 0;


    //manage the minigames

    public MiniGameManager miniGameScript;

    //modify scores of players
    public ScoreManager scoreScript;


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
    private void RoundCheck() // compares currentTurn with player count, exceeds, start minigame
    {
        var playerCount = GameObject.FindGameObjectsWithTag("Player").Length;

        if(playerCount < currentTurn) // turn on el minigame
        {
            StartCoroutine(miniGameScript.StartMiniGame()); // running minigame coroutine to advise players, and spawn game.
        }
    }

    public void RunTurn(GameObject player) // updates turn for players
    {
        var playerTurn = player.GetComponent<PlayerControls>().turnOrder;
        if(playerTurn == currentTurn) //check to see if its the players turn
        {
            //move player
            player.GetComponent<PlayerControls>().Moving(2); // 2 is the wait time between moves
        }
        //check to see if turn has gone over the playercount
        RoundCheck();
    }
}
