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

    private PlayerInputManager manager; // disable joining mechanics

    [Header("Player")]
    public Transform[] spawn; // spawn player in correct spot
    public Transform waitSpawn; // spawn player in waiting room
    public GameObject[] player; // player append variable
    public List<Color> playerColor; // colors for players to differentiate who they are

    [Header("Player Passdowns")] // pass down variables to players
    public GameObject pauseMenu;
    public TurnManager turn; // start up turns
    public MovementManager moveManage;
    public CameraControl cam;
    [Header("Debug")]
    public bool miniGameTesting = false;
    private void Start()
    {
        manager = GetComponent<PlayerInputManager>();
        if(!turn)
        {
            turn = GameObject.FindGameObjectWithTag("Turn Manager").GetComponent<TurnManager>();
        }
        if(!moveManage)
        {
            moveManage = GameObject.FindGameObjectWithTag("Movement Manager").GetComponent<MovementManager>();
        }
        if(!cam)
        {
            cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraControl>();
        }

        if (manager.playerPrefab == null)
        {
            manager.playerPrefab = Resources.Load("Experimental/Player") as GameObject;
        }
    }
    public void PlayerArray(PlayerInput input) // adds to player array for minigames
    {
        var spawnPlayer = input.gameObject; // grabbing object of player

        Debug.Log("Appending Player");
        player = player.Append(spawnPlayer).ToArray();

        // add their correct turn order as well
        Debug.Log("Player Length is:" + (player.Length - 1).ToString());
        spawnPlayer.GetComponent<PlayerStats>().turnOrder = player.Length - 1;

        spawnPlayer.GetComponent<PlayerControls>().pauseMenu = pauseMenu; // pass to player

        // giving players the turn manager and movement manager to call certain variables in multiplayer
        var plrControl = spawnPlayer.GetComponent<PlayerControls>();

        plrControl.turnScript = turn;
        plrControl.moveManage = moveManage;
        plrControl.cam = cam;

        // setting player's color
        spawnPlayer.GetComponentInChildren<SpriteRenderer>().color = playerColor.ElementAt(player.Length - 1);
    }
    public void SinglePlayer(PlayerInput input)
    {
        var stat = input.gameObject.GetComponent<PlayerStats>();

        stat.singlePlayer = true;
        
        input.gameObject.transform.position = new Vector3(input.gameObject.transform.position.x, input.gameObject.transform.position.y, 5);

        // gameswitch to make jumps exists

        var movement = input.gameObject.GetComponent<PlayerMovement>();

        movement.rb = input.gameObject.GetComponent<Rigidbody2D>();
        movement.GameSwitch(false);
    }
    public void StartMultiplayer() // Move player in select location
    {
        for(int i = 0; i < player.Length; i++)
        {
            var plr = player[i].gameObject;
            var playerSpawn = plr.GetComponent<PlayerStats>().turnOrder;

            plr.transform.position = spawn[playerSpawn].position; // move players to map
            plr.GetComponent<PlayerMovement>().GameSwitch(false); // prevent player moving
        }
        turn.SetTurn(0);
    }
    public void MultiPlayer(PlayerInput input)
    {
        input.gameObject.transform.position = waitSpawn.transform.position;

        // allowing player movement
        var movement = input.gameObject.GetComponent<PlayerMovement>();

        movement.rb = input.gameObject.GetComponent<Rigidbody2D>();
        movement.GameSwitch(true);
    }

    public IEnumerator StartGame() // start the game, run DisableJoin
    {
        yield return new WaitForSeconds(2);
        DisableJoin(); // turn off players from being able to join the game
        yield return new WaitForSeconds(5); // wait 5 seconds before going on

        turn.SetTurn(0); //allow players to begin doing their turns
        // ^ to be changed to after cutscene overlooking map

        // multiplayer procedure begins here
        StartMultiplayer();

    }
    private void DisableJoin() // disable players joining the game.
    {
        manager.DisableJoining(); // disables joining from the players end
    }
    public void GameOver() // for single player
    {
        //Reset turn order
        turn.SetTurn(0);

        // siginify in single player that player has lost
        var uiManager = GameObject.FindGameObjectWithTag("PlayerUIManager").GetComponent<PlayerUIManager>();

        uiManager.gameOverUI.SetActive(true);
    }
}
