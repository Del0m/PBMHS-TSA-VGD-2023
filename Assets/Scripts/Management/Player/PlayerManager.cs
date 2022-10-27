//armin delmo, PlayerManager.cs
//the purpose of this program is to collect all the players for counting and removal purposes.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public GameObject playerManager;
    public int playerCount = 0;
    public GameObject[] players;

    private void Start()
    {
        playerManager = this.gameObject;
    }
    //grab all players, chuck into array
    public void GetPlayers()
    {
        for (int i = 0; i > playerCount; i++)
        {
            players[i] = this.transform.GetChild(i).gameObject;
        }
    }

  


}
