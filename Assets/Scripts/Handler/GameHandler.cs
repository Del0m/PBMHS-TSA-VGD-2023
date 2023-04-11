//armin delmo, 11/13/22. Purpose of script is to handle minigame startup, grabbing players, starting game, etc.
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class GameHandler : MonoBehaviour
{
    //player array to modify rigidbodies and teleport
    [Header("Player Important Variables")]
    public List<GameObject> player;
    public PlayerManager plrManage; // to import player array

    public MovementManager movementManager;
    //array of spawns
    public GameObject[] teleport;

    //score
    public List<int> gameScore;

    public int winner; // if need be 

    //useless in every game that doesn't spawn something
    public Transform[] border; // array holding the borders

    [Header("Basic Settings")]
    public bool timeLimit;
    //[HideInInspector] // don't need to see it, clutter
    [Header("UI")]
    public PlayerUIManager uiManager;
    public GameObject tutorialScreen; // to be individually selected depending on what minigame

    public TurnManager turn;

    [Header("Camera + Settings")] // to proeperly position the camera in a minigame
    public CameraControl cam;
    public Transform camPos;
    public int fov;

    void Start()
    {
        turn = GameObject.FindGameObjectWithTag("Turn Manager").GetComponent<TurnManager>();
        uiManager = GameObject.FindGameObjectWithTag("PlayerUIManager").GetComponent<PlayerUIManager>();
        try
        {
            if (plrManage) { }; // checking player manager if it isn't null
            if(turn) { }; // checking if turn manager isn't null
            if (uiManager) { } ; // checking to see if ui manager isn't null
        }
        catch (System.Exception)
        {
            Debug.LogError("Manager not found, check inspector and reset!");
        }
    }
    /*
    public virtual void IncreaseDifficulty() // increase the difficulty of the game in single player
    {
        multi = spManage.multiplier;
        // put stuff in here in other programs idk
    }
    */
    public void TeleportPlayers() // void to collect all players on the map, and place them in the according location in minigame
    {
        
        player = plrManage.player;

        for(int i = 0; i < player.Count; i++) // for loop to set all players in correct position for game
        {
            Debug.Log("moving player " + i+1 + " to scene");

            player[i].transform.position = teleport[i].transform.position; // set position for player in minigame
            player[i].GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static; // prevent movement until necessary
        }
    }
    
    public virtual IEnumerator PreGameRoutine() // routine to run when before the minigame to see if anything needs to be added to the game.
    {
        TeleportPlayers(); // teleport players into the game

        uiManager = GameObject.FindGameObjectWithTag("PlayerUIManager").GetComponent<PlayerUIManager>();

        // move the camera to the minigame
        MoveCamera(camPos, fov);

        StartCoroutine(uiManager.ShowUI(tutorialScreen));

        StartCoroutine(uiManager.CountDown(3, uiManager.countdownUI));
        yield return new WaitForEndOfFrame();
    }
    public virtual IEnumerator StartGame() // teleports players into minigame
    {
        yield return new WaitForEndOfFrame();
        StartCoroutine(PreGameRoutine());

        // for loop to allow all players controls, add this in the override 
        /*
        for (int i = 0; i < player.Count; i++)
        {
            var playerMovement = player[i].GetComponent<PlayerMovement>();

            playerMovement.GameSwitch(true);
        }
        */
        yield return null;
    }
    public void MoveCamera(Transform pos, int fov)
    {
        if(!cam) // check if the cam is found already
        {
            cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraControl>();
        }
        cam.TeleportCamera(pos, fov);
    }
    public void TeleportBack() // bring players back to their spawn point
    {
        movementManager = GameObject.FindGameObjectWithTag("Movement Manager").GetComponent<MovementManager>();

        for (int i = 0; i < player.Count; i++) // for loop to set all players in correct position for game
        {
            var playerStat = player[i].GetComponent<PlayerStats>(); // calling player's home position
            
            var tile = movementManager.CallTile(playerStat.position);
            player[i].transform.position = tile.transform.position; // set position for player in board

            player[i].GetComponent<PlayerMovement>().GameSwitch(false, false, false);
            player[i].GetComponent<PlayerControls>().hasRan = false;

            if (turn)
            {
                turn.SetTurn(0);
            }
            else
            {
                Debug.LogError("turn manager is not found!");
            }
        }
        // collect first player's position to return the camera to that location
        MoveCamera(movementManager.CallTile(player[0].GetComponent<PlayerStats>().position), fov);
    }

    public IEnumerator EndGame(int winner)
    {
        TeleportBack();
        plrManage.TransitionGame(winner);

        uiManager.ChangeUI(false, uiManager.healthBarUI); // reset the UI
        uiManager.ChangeUI(false, uiManager.loseUI); // reset the losing scren


        // set players back to their tiles

        for(int i = 0; i < player.Count; i++) // teleport players back to their tiles
        {
            var plrStat = player[i].GetComponent<PlayerStats>();
            var pos = plrStat.position;
            var moveManage = GameObject.FindGameObjectWithTag("Movement Manager").GetComponent<MovementManager>();

            player[i].GetComponent<PlayerMovement>().GameSwitch(false, false, false);

            player[i].gameObject.transform.position = moveManage.CallTile(pos).gameObject.GetComponent<Tile>().playerPositions[plrStat.turnOrder].position;
        }
        cam.TeleportCamera(player[0].transform.position, 20); // bring camera back to board frame.

        // ending game
        Destroy(gameObject, 1f);
        yield return null;
    }
    public int CheckWinner() // returns the player who got the most points
    {
        var highestScore = -9;
        var winner = 9;
        for(int i = 0; i < player.Count; i++)
        {
            if(highestScore < gameScore[i])
            {
                highestScore = gameScore[i];
                Debug.Log("Highest score is:" + highestScore);
                winner = i;
            }
        }
        return winner;
    }
}
