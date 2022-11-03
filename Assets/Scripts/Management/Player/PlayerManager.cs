//armin delmo, PlayerManager.cs
//the purpose of this program is to collect all the players for counting and removal purposes.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerManager : MonoBehaviour
{
    //player tracking variables
    public int playerCount = 0;
    private GameObject[] players;

    public TurnManager turnManager;

    public int currentTurn;


    private void Start() // intializing scripts that will be used
    {
        this.gameObject.tag = "Player Manager";//change this object to have player manager tag

        turnManager = GameObject.FindGameObjectWithTag("Turn Manager").GetComponent<TurnManager>(); // turn manager script for updating purposes
        currentTurn = turnManager.currentTurn; // grab turn from mnanager script
    }

    private void Update() // me giving up on an elegant solution, will constantly update numbers since NOTHING ELSE WORKS
    {
        GetPlayers();
    }

    public void GetPlayers() // grab players, chuck in array, I actually give up... it doesn't want to grab players unless its in update, RIP.
    {
        players = GameObject.FindGameObjectsWithTag("Player"); // grab player, put in array
        playerCount = players.Length; // update count with current players in array

        turnManager.GetPlayers(players); // update player array in turnManager
    }
}
