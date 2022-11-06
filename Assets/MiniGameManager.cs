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
    private void Awake()
    {
        this.gameObject.tag = "Mini Game Manager";
    }
    // Update is called once per frame
    void Update()
    {
        
    }


    public void StartMiniGame(GameObject[] playerCall) //called from TurnManager, this will grab all given players and bring them to a minigame
    {
        Debug.Log("Starting Minigame!");
        //update current player array
        LoadPlayers(playerCall);

        //changing player controls
        for (int i = 0; i >= players.Length; i++)
        {
            players[i].GetComponent<PlayerControls>().gameplayInput.actions.FindActionMap("boardGamePlay").Disable();
            players[i].GetComponent<PlayerControls>().gameplayInput.actions.FindActionMap("miniGamePlay").Enable();
        }


    }

    private void LoadPlayers(GameObject[] player)
    {
        //turning given player array to current MiniGameManager player array
        players = player;

    }


}
