//armin delmo
//the purpose of this script is to disable players joining the game once the game has begun.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    [HideInInspector]
    public PlayerInputManager manager; // disable joining mechanics
    public PlayerUIManager uiManager; // modify the UI
    [Header("Player")]
    public Transform[] spawn; // spawn player in correct spot
    public Transform waitSpawn; // spawn player in waiting room
    public List<GameObject> player; // player append variable
    public List<Color> playerColor; // colors for players to differentiate who they are

    [Header("Player Passdowns")] // pass down variables to players
    public GameObject pauseMenu;
    public TurnManager turn; // start up turns
    public MovementManager moveManage;
    public CameraControl cam;

    public bool singlePlayer;
    public double multiplier;

    private void Start()
    {
        manager = GetComponent<PlayerInputManager>();

        if (!manager.playerPrefab)
        {
            manager.playerPrefab = Resources.Load("Experimental/Player") as GameObject;
        }
    }
    public virtual void SpawnPlayer(PlayerInput input) // adds to player array for minigames
    {

        var spawnPlayer = input.gameObject; // grabbing object of player
        

        player.Add(spawnPlayer); // add player into the count

        // add their correct turn order as well, add this to multiplayer
        /*
        spawnPlayer.GetComponent<PlayerStats>().turnOrder = (player.Count - 1);

        */
    }
    public virtual void SetPlayer(GameObject spawnPlayer)
    {
        spawnPlayer.GetComponent<PlayerControls>().pauseMenu = pauseMenu; // pass to player

        // giving players the turn manager and movement manager to call certain variables in multiplayer
        var plrControl = spawnPlayer.GetComponent<PlayerControls>();

        plrControl.turnScript = turn;
        plrControl.moveManage = moveManage;
        plrControl.cam = cam;

        spawnPlayer.GetComponent<PlayerStats>().singlePlayer = singlePlayer;
        // setting player's color, put this in multiplayer manager
        /* if(!singlePlayer) 
         {
             spawnPlayer.GetComponentInChildren<SpriteRenderer>().color = playerColor.ElementAt(player.Length - 1);
         }
        */
    }
    
    public virtual void TransitionGame(int won) // move the game back to the board
    {
        Debug.Log("Running basic transition game.");
        uiManager.UIPopUpWrapper(uiManager.successUI, won + 1);
    }
}
