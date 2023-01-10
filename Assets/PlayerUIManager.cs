using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerUIManager : MonoBehaviour
{
    public GameObject[] playerUI; // the UI that shows the player's wins, icon and buffs

    // this will be ran in the PlayerManager
    public void InitalizeUI() // this is to hide UI that won't be used in the game due to a lack of plrs
    {
        var playerCount = GameObject.FindGameObjectsWithTag("Player").Length; // grabbing the amount of players in the game

        //hide all ui for loop
        for(int i = 0; i < playerUI.Length; i++)
        {
            playerUI[i].SetActive(false);
        }

        // unhide used UI for loop
        for(int i = 0; i < playerCount; i++)
        {
            playerUI[i].SetActive(true);
        }
    }
}
