/*armindelmo MiniGameManager.cs
this script will load once all players have finished their turns, it will begin a new round and start a minigame for the players to participate in.

*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGameManager : MonoBehaviour
{
    //players
    private GameObject[] players; // player array to change controls, bring to map, etc.

    //variables for minigame
    public bool hasStarted = false;

    //array for all available minigames
    private GameObject minigame;
    private void Awake()
    {
        this.gameObject.tag = "Mini Game Manager";
    }


    public void StartMiniGame(GameObject[] playerCall) //called from TurnManager, this will grab all given players and bring them to a minigame
    {
        Debug.Log("Starting Minigame!");
        players = playerCall;
        //changing player controls
        for (int i = 0; i < players.Length; i++)
        {
            Debug.Log("Changing Controls on Player " + i);
            players[i].GetComponent<PlayerControls>().PlayerMiniGameMode(true);

        }
        if(hasStarted != true)
        {
            // grabbing minigame to choose
            LoadGamesFromFile();
            hasStarted = true;
        }



    }
    public void EndMiniGame() //kills all minigames, brings back controls to players
    {
        KillGamesFromFile(); // deletes minigame
        for (int i = 0; i >= players.Length - 1; i++) // brings plrs back to board.
        {
            Debug.Log("Changing Controls on Player " + i);
            players[i].GetComponent<PlayerControls>().PlayerMiniGameMode(false);


            hasStarted = false;
        }
    }

    public void GetPlayers(GameObject[] player)
    {
        //turning given player array to current MiniGameManager player array
        players = player;

    }
    private void LoadGamesFromFile() // resource.load all games, pick one, deload
    {
        Debug.Log("Loading!");
        GameObject minigameInstance = Instantiate(Resources.Load("Prefabs/Mini-Games/Minigame A", typeof(GameObject))) as GameObject;
        minigameInstance.GetComponent<GameHandler>().JumpStart();
    }
    private void KillGamesFromFile()
    {
        var gameOut = GameObject.FindGameObjectWithTag("Minigame");
        Destroy(gameOut);
    }


}
