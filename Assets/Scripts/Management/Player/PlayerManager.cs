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

    private void Start() // intializing scripts that will be used
    {
        turnManager = GameObject.FindGameObjectWithTag("Turn Manager").GetComponent<TurnManager>(); // turn manager script for updating purposes

    }

    public void GetPlayers() // grab players, chuck in array
    {
        players = GameObject.FindGameObjectsWithTag("Player"); //collects all players in scene
        playerCount = GameObject.FindGameObjectsWithTag("Player").Length; // update player count

        //update turn manager
        turnManager.GetPlayers();
    }




}
