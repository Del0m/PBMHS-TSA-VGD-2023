/*
    Purpose: A collectable item for players, that gives them a score
    Author: Yahir Bonilla
    Version: January 15, 2023
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [Header("Coin Parameters")]
    public int scoreAmount = 1;

    //Take reference of the minigame script
    private RunningScamper minigame;

    // Start is called before the first frame update
    void Start()
    {
        grabMinigame();
    }

    void grabMinigame(){
        //Grab minigame script from scene
        GameObject miniObj = GameObject.FindGameObjectWithTag("Minigame");
        if(miniObj != null )
        {
            minigame = miniObj.GetComponent<RunningScamper>();
        }
        else
        {
            Debug.LogError("Minigame not found!");
        }
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            int i = checkForPlayer(other.gameObject);
            if (i > -1) 
            {
                minigame.ScorePlayer(i, scoreAmount);
                Destroy(this.gameObject);
            }
        }
    }

    int checkForPlayer(GameObject player)
    {
        int playerIndex = -1;
        GameObject[] players;

        if(minigame != null)
        {
            players = minigame.player;
            for(int i = 0; i < players.Length; i++)
            {
                if(player == players[i])
                {
                    playerIndex = i;
                    break;
                }
                else
                {
                    playerIndex = -1;
                }
            }
        }
        else
        {
            Debug.LogError("Minigame not found!");
            grabMinigame();
            return -1;
        }

        return playerIndex;
    }
}
