//armin delmo, PlayerManager.cs
//the purpose of this program is to collect all the players for counting and removal purposes.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public int playerCount = 0;
    public GameObject[] players;

    private void Update()
    {
        GetPlayers();
    }
    public void GetPlayers() // grab players, chuck in array
    {
        for (int i = 0; i > playerCount+200; i++)
        {
            players[i] = GameObject.FindGameObjectWithTag("Player"); //collects all players in scene
            if (players[i] != null)
            {
                print("Player Found!");
            }
            playerCount = GameObject.FindGameObjectsWithTag("Player").Length; // update player count
        }
    }

  


}
