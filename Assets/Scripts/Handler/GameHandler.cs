//armin delmo, 11/13/22. Purpose of script is to handle minigame startup, grabbing players, starting game, etc.
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class GameHandler : MonoBehaviour
{
    //player array to modify rigidbodies and teleport
    [Header("Player Important Variables")]
    public List<GameObject> player;
    public PlayerManager plrManage; // to import player array
    public GameUI gameUI; // manages time limit for games if needed.
    public MovementManager movementManager;
    //array of spawns
    public GameObject[] teleport;

    //score
    public List<int> gameScore = new List<int>(4);

    public int winner; // if need be 

    public BoxCollider2D boundary; // holds the boundaries of where things can spawn.

    [HideInInspector]
    public double multiplier; // used for quick rounds / single player

    [Header("UI")]
    public PlayerUIManager uiManager;
    public GameObject tutorialScreen; // to be individually selected depending on what minigame

    public TurnManager turn;

    [Header("Camera + Settings")] // to properly position the camera in a minigame
    public CameraControl cam;
    public Transform camPos;
    public int fov;

    [HideInInspector]
    public int minimumToWin;
    public bool noWinner; // provide no winner if the game needs it
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

    public virtual void IncreaseDifficulty()
    {
        // make an override that changes the competitive variables of the game

        multiplier = plrManage.multiplier; // find current difficulty
    }

    public void TeleportPlayers() // void to collect all players on the map, and place them in the according location in minigame
    {
        if(teleport.Length == 0) // check if the teleports have been initalized
        {
            teleport = GameObject.FindGameObjectsWithTag("Teleport");
        }
        player = plrManage.player;

        for(int i = 0; i < player.Count; i++) // for loop to set all players in correct position for game
        {
            player[i].transform.position = teleport[i].transform.position; // set position for player in minigame
            player[i].GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static; // prevent movement until necessary
        }
    }
    public virtual IEnumerator PreGameRoutine() // routine to run when before the minigame to see if anything needs to be added to the game.
    {
        // set the difficulty with IncreaseDifficulty() in the override

        TeleportPlayers(); // teleport players into the game

        uiManager = GameObject.FindGameObjectWithTag("PlayerUIManager").GetComponent<PlayerUIManager>();

        // move the camera to the minigame
        MoveCamera(camPos, fov);

        StartCoroutine(uiManager.ShowUI(tutorialScreen));

        StartCoroutine(uiManager.CountDown(3, uiManager.countdownUI));

        // disabling non-minigame UI
        plrManage.SetObjects(false);
        yield return new WaitForEndOfFrame();
    }
    public virtual IEnumerator StartGame() // teleports players into minigame
    {
        // this must be changed every game mode, and have the base.StartGame() run AFTER your code

        StartCoroutine(PreGameRoutine());
        yield return new WaitForEndOfFrame();

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
        cam.destination = pos; // to prevent error drop
        cam.TeleportCamera(pos.position, fov);
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
    public void UpdateWin(int winner) // for the player to have their wins updated
    {
        uiManager.playerUI[winner].GetComponent<PlayerUI>().UpdateWins(player[winner].GetComponent<PlayerStats>().wins);
    }

    public IEnumerator EndGame(int winner)
    {
        
        TeleportBack();

        uiManager.ChangeUI(false, uiManager.healthBarUI); // reset the UI
        uiManager.ChangeUI(false, uiManager.loseUI); // reset the losing scren



        // set players back to their tiles

        for (int i = 0; i < player.Count; i++) // teleport players back to their tiles
        {
            var plrStat = player[i].GetComponent<PlayerStats>();
            var pos = plrStat.position;
            var moveManage = GameObject.FindGameObjectWithTag("Movement Manager").GetComponent<MovementManager>();

            player[i].GetComponent<PlayerMovement>().GameSwitch(false, false, false);

            player[i].gameObject.transform.position = moveManage.CallTile(pos).position;
        }
        cam.TeleportCamera(player[0].transform.position, 20); // bring camera back to board frame.


        // try statement to prevent single player loss from reaching a negative array error
        try
        {
            if(!noWinner) // check if the game needs to manually award player
            {
                // award player a point if they "won"
                player[winner].GetComponent<PlayerStats>().wins++;
            }
        }
        catch (System.Exception)
        {
            // don't run anything here, this is to prevent a negative array exception from coming up when the single player loses.
        }
        UpdateWin(winner); // update the UI for multiplayer

        // enabling non-minigame UI
        plrManage.SetObjects(true);

        // transition the game to have the success ascreen, singleplayer updates the players level.
        plrManage.TransitionGame(winner);

        // ending game
        Destroy(gameObject, 1f);
        yield return null;
    }
    public Vector2 RandPosition() // randomize boundary spawns with players
    {
        // this will find something in bounds, and will return a value in such bounds.

        // randomize
        var randVect = new Vector2(
            Random.Range(boundary.bounds.min.x, boundary.bounds.max.x),
            Random.Range(boundary.bounds.min.y, boundary.bounds.max.y)
            );

        return randVect;
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
        if(winner < minimumToWin && player.Count == 1) // make players lose on single player
        {
            return -1; // return no winner so the game ends.
        }

        return winner;
    }
}
