//armin delmo, 11/13/22. Purpose of script is to handle minigame startup, grabbing players, starting game, etc.

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using UnityEngine.VFX;
using UnityEngine.WSA;

public class GameHandler : MonoBehaviour
{
    //player array to modify rigidbodies and teleport
    public GameObject[] player;

    //array of spawns
    public GameObject[] teleport;

    //score
    public int[] gameScore = { 0,0,0,0 };

    public int winner; // if need be 

    public Transform[] border; // array holding the borders

    private void Start()
    {
        teleport = GameObject.FindGameObjectsWithTag("Teleport");
    }
    public void TeleportPlayers() // void to collect all players on the map, and place them in the according location in minigame
    {
        player = GameObject.FindGameObjectsWithTag("Player");
        teleport??= GameObject.FindGameObjectsWithTag("Teleport"); // check if null, replace spawns

        for(int i = 0; i < player.Length; i++) // for loop to set all players in correct position for game
        {
            player[i].transform.position = teleport[i].transform.position; // set position for player in minigame
        }
    }
    public void TutorialUI()
    {
        // nothing here, Yahir, this is your spot 
    }
    public IEnumerator StartGame(bool enable) // teleports players into minigame
    {
        TeleportPlayers(); // teleport players into the game

        // for loop to allow all players controls
        for(int i = 0; i < player.Length; i++)
        {
            var playerMovement = player[i].GetComponent<PlayerMovement>();

            playerMovement.GameSwitch(enable);
        }
        // add UI pause here []
        TutorialUI(); // Yahir

        yield return new WaitForSeconds(3);
    }

    public IEnumerator StartGame(bool enable, bool topDown) // teleports players into minigame; allow topdown
    {
        TeleportPlayers(); // teleport players into the game

        // for loop to allow all players controls
        for (int i = 0; i < player.Length; i++)
        {
            var playerMovement = player[i].GetComponent<PlayerMovement>();

            playerMovement.GameSwitch(enable, topDown);
        }
        // add UI pause here []
        TutorialUI(); // Yahir

        yield return new WaitForSeconds(3);
    }
    public IEnumerator StartGame(bool enable, bool topDown, bool pick) // teleports players into minigame; allow topdown
    {
        TeleportPlayers(); // teleport players into the game

        // for loop to allow all players controls
        for (int i = 0; i < player.Length; i++)
        {
            var playerMovement = player[i].GetComponent<PlayerMovement>();

            playerMovement.GameSwitch(enable, topDown, pick);
        }
        // add UI pause here []
        TutorialUI(); // Yahir

        yield return new WaitForSeconds(3);
    }
    public virtual IEnumerator EndGame(int winner) // coroutine to end the game as a player has won.
    {
        for(int i = 0; i < player.Length; i++)
        {
            var playerStat = player[i].GetComponent<PlayerStats>();
            if(playerStat.turnOrder == winner)
            {
                playerStat.wins++;
            }
        }
        Debug.Log("Game has ended.");
        yield return null;
    }
    public virtual IEnumerator EndGame(int loser, bool winnersWin)
    {
        for(int i = 0; i < player.Length; i++) // for loop, loser loses point, everyone wins!
        {
            var playerStat = player[i].GetComponent<PlayerStats>();
            if(playerStat.turnOrder == loser)
            {
                playerStat.wins--;
            }
            else if(winnersWin)
            {
                playerStat.wins++;
            }
        }
        Debug.Log("Game has ended");
        yield return null;
    }
}
