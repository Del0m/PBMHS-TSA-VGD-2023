//armin delmo, PlayerManager.cs
//the purpose of this program is to collect all the players for counting and removal purposes.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerManager : MonoBehaviour
{
    //player tracking variables
    public int playerCount = 0;
    public GameObject[] players;

    public TurnManager turnManager;

    public int currentTurn;


    private void Start() // intializing scripts that will be used
    {
        this.gameObject.tag = "Player Manager";//change this object to have player manager tag

        turnManager = GameObject.FindGameObjectWithTag("Turn Manager").GetComponent<TurnManager>(); // turn manager script for updating purposes
        currentTurn = turnManager.currentTurn; // grab turn from mnanager script
        GetPlayers();
    }

    public void GetPlayers() // grab players, chuck in array
    {
        Debug.Log("Running GetPlayers()");
        players = GameObject.FindGameObjectsWithTag("Player"); //collects all players in scene
        playerCount = players.Length; // update player count

        //turnManager.GetPlayers(); //update turn manager
    }
}
