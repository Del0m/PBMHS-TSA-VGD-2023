//armin delmo, 11/13/22. Purpose of script is to handle minigame startup, grabbing players, starting game, etc.
using System.Collections;
using TMPro;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    //player array to modify rigidbodies and teleport
    [Header("Player Important Variables")]
    public GameObject[] player;
    public PlayerManager plrManage; // to import player array

    //array of spawns
    public GameObject[] teleport;

    //score
    public int[] gameScore;

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

    [Header("SinglePlayer Specific")] // only to be ticked in the single player mode, do not turn on otherwise
    public TextMeshProUGUI level; // increase level on game
    public bool singlePlayer;
    public Transform spSpawn;

    // multipliers to make game harder
    public SinglePlayerManager spManage;
    public double multi;

    private bool ended; // changes, prevents end game from being ran several times
    [HideInInspector]
    public bool allowCameraFollow = false; //Used to tell the camera to follow the players, by default it's turned off (Set true only on minigame script if needea
    void Start()
    {
        turn = GameObject.FindGameObjectWithTag("Turn Manager").GetComponent<TurnManager>();
        uiManager = GameObject.FindGameObjectWithTag("PlayerUIManager").GetComponent<PlayerUIManager>();
        //mg = GameObject.FindGameObjectWithTag("Mini Game Manager").GetComponent<MiniGameManager>();
        teleport = GameObject.FindGameObjectsWithTag("Teleport");

        // function to increase difficulty for players
        if(singlePlayer)
        {
            IncreaseDifficulty();
        }
    }
    public virtual void IncreaseDifficulty() // increase the difficulty of the game in single player
    {
        multi = spManage.multiplier;
        // put stuff in here in other programs idk
    }
    public void TeleportPlayers() // void to collect all players on the map, and place them in the according location in minigame
    {
        plrManage = GameObject.FindGameObjectWithTag("Player Manager").GetComponent<PlayerManager>();
        if (!allowCameraFollow)
        {
            cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraControl>();
            cam.TeleportCamera(camPos, fov); // change camera into minigame spot
        }

        player = plrManage.player;
        teleport = GameObject.FindGameObjectsWithTag("Teleport"); // check if null, replace spawns

        for(int i = 0; i < player.Length; i++) // for loop to set all players in correct position for game
        {
            Debug.Log("moving player to scene");
            player[i].transform.position = teleport[i].transform.position; // set position for player in minigame
            player[i].GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static; // prevent movement until necessary
        }
    }
    bool isntActing;
    public IEnumerator MinigameUI(bool enter, GameObject screen)
    {
        uiManager.ChangeUI(enter); // enable the minigame ui
        screen.SetActive(true);
        isntActing = true;

        // slow down time to allow game to not go on
        Time.timeScale = 0.0001f; // go REALLY slow

        while (isntActing)
        {
            if(!screen.activeInHierarchy) // enable UI when it is not on correctly
            {
                screen.SetActive(true);
            }
            Debug.Log("running loop");
            // don't do anything besides check
            if(player[0].GetComponent<PlayerMovement>().acting == true)
            {
                Debug.Log("Acting found!");
                isntActing = false;
                screen.SetActive(false); // bring down UI
                Time.timeScale = 1f; // go back to speed

                break;
            }
            yield return new WaitForEndOfFrame();
        }

    }
    public virtual IEnumerator PreGameRoutine() // routine to run when before the minigame to see if anything needs to be added to the game.
    {
        yield return new WaitForEndOfFrame();
        cam.forgetDestination();
        TeleportPlayers(); // teleport players into the game

        var scoreArray = new int[player.Length];
        gameScore = scoreArray;
        uiManager = GameObject.FindGameObjectWithTag("PlayerUIManager").GetComponent<PlayerUIManager>();

        StartCoroutine(MinigameUI(true, tutorialScreen));

        StartCoroutine(uiManager.CountDown(3, uiManager.countdownUI));
        yield return new WaitForEndOfFrame();
    }
    public IEnumerator StartGame(bool enable) // teleports players into minigame
    {
        yield return new WaitForEndOfFrame();
        StartCoroutine(PreGameRoutine());

        yield return new WaitForSeconds(3.1f);

        //StartCoroutine(PreGameRoutine());

        // for loop to allow all players controls
        for (int i = 0; i < player.Length; i++)
        {
            var playerMovement = player[i].GetComponent<PlayerMovement>();

            playerMovement.GameSwitch(enable);
        }
        yield return null;
    }

    public IEnumerator StartGame(bool enable, bool topDown) // teleports players into minigame; allow topdown
    {

        StartCoroutine(PreGameRoutine());

        yield return new WaitForSeconds(3.1f);

        // for loop to allow all players controls
        for (int i = 0; i < player.Length; i++)
        {
            var playerMovement = player[i].GetComponent<PlayerMovement>();

            playerMovement.GameSwitch(enable, topDown);
        }
        yield return null;
    }
    public IEnumerator StartGame(bool enable, bool topDown, bool pick) // teleports players into minigame; allow topdown
    {

        StartCoroutine(PreGameRoutine());

        yield return new WaitForSeconds(3.1f);

        // for loop to allow all players controls
        for (int i = 0; i < player.Length; i++)
        {
            var playerMovement = player[i].GetComponent<PlayerMovement>();

            playerMovement.GameSwitch(enable, topDown, pick);
        }
        yield return null;
    }
    public void TeleportBack() // bring players back to their spawn point
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraControl>();
        cam.TeleportCamera(camPos, fov); // change camera into minigame spot

        var plr = GameObject.FindGameObjectsWithTag("Player");
        teleport = GameObject.FindGameObjectsWithTag("Teleport"); // check if null, replace spawns

        for (int i = 0; i < plr.Length; i++) // for loop to set all players in correct position for game
        {
            if(!singlePlayer)
            {
                var playerStat = plr[i].GetComponent<PlayerStats>(); // calling player's position
                var movementManager = GameObject.FindGameObjectWithTag("Movement Manager").GetComponent<MovementManager>();

                // moving player back to tile
                var tile = movementManager.CallTile(playerStat.position);
                plr[i].transform.position = tile.transform.position; // set position for player in board
                plr[i].GetComponent<PlayerMovement>().GameSwitch(false ,false, false);
                plr[i].GetComponent<PlayerControls>().hasRan = false;
                if(turn != null){
                    turn.SetTurn(0);
                }else{
                    Debug.LogError("The turn manager is missing");
                }
            }
            else
            {
                plr[i].transform.position = new Vector2(spSpawn.position.x,spSpawn.position.y); // bring player back to map
                plr[i].GetComponent<PlayerMovement>().GameSwitch(false);
                cam.TeleportCamera(spSpawn, 20); // set position of camera 
                
            }
        }

        pl = plr[0];
    }

    private GameObject pl = null;

    public virtual IEnumerator EndGame()
    {
        TeleportBack();
        Debug.Log("Game has ended.");

        if(singlePlayer)
        {
            //increase level
            spManage.IncreaseLevel();
        }
        uiManager.ChangeUI(false, uiManager.healthBarUI); // reset the UI

        uiManager.UIPopUpWrapper(uiManager.successUI);

        // set players back to their tiles
        if(!singlePlayer)
        {
            var plr = GameObject.FindGameObjectsWithTag("Player");
            for(int i = 0; i < plr.Length; i++) // teleport players back to their tiles
            {
                var pos = plr[i].GetComponent<PlayerStats>().position;
                var moveManage = GameObject.FindGameObjectWithTag("Movement Manager").GetComponent<MovementManager>();

                plr[i].gameObject.transform.position = moveManage.CallTile(pos).position;
            }
        }

        if(!singlePlayer && pl != null){
            //Call camera tp
            Vector3 pos = new Vector3(pl.transform.position.x, pl.transform.position.y, -100f);

            cam.TeleportCamera(pos, 20);
        }

        Destroy(gameObject, 1f);

        yield return null;
    }
    public virtual IEnumerator EndGame(bool won) // single player endgame routine
    {
        if (ended) { yield break; }
        ended = true;
        if (won)
        {
            // increase level values; bring back player
            StartCoroutine(EndGame());
        }
        else
        {
            var plrManage = GameObject.FindGameObjectWithTag("Player Manager").GetComponent<PlayerManager>();
            plrManage.GameOver();
        }
        Debug.Log("Game has ended.");
        // adding level update
        if(singlePlayer)
        {
            uiManager.UpdateLevel(spManage.level); // updating the player's level in the game
        }
        Destroy(gameObject, 1f);
        yield return null;
    }
    public virtual IEnumerator EndGame(int winner) // coroutine to end the game as a player has won.
    {
        if (ended) { yield break; }
        ended = true;

        yield return new WaitForSeconds(2); 
        for(int i = 0; i < player.Length; i++)
        {
            var playerStat = player[i].GetComponent<PlayerStats>();
            if(playerStat.turnOrder == winner)
            {
                Debug.Log(playerStat.turnOrder + " has won!");
                playerStat.wins++;
            }
            else
            {
                // do nothing
            }
        }
        Debug.Log("Game has ended.");
        StartCoroutine(EndGame());
        Destroy(gameObject, 1f);

        yield return null;
    }
    public virtual IEnumerator EndGame(int loser, bool winnersWin)
    {
        if (ended) { yield break; }
        ended = true;
        for (int i = 0; i < player.Length; i++) // for loop, loser loses point, everyone wins!
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
        StartCoroutine(EndGame());
        Destroy(gameObject, 1f);

        yield return null;
    }
    public int CheckWinner() // returns the player who got the most points
    {
        var highestScore = -9; 
        for(int i = 0; i < player.Length; i++)
        {
            if(highestScore < gameScore[i])
            {
                highestScore = gameScore[i];
            }
        }
        return highestScore;
    }
}
