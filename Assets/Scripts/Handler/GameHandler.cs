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

    //[HideInInspector] // don't need to see it, clutter
    [Header("UI")]
    public PlayerUIManager uiManager;

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
    [HideInInspector]
    public bool allowCameraFollow = false; //Used to tell the camera to follow the players, by default it's turned off (Set true only on minigame script if needea
    void Start()
    {
        uiManager = GameObject.FindGameObjectWithTag("PlayerUIManager").GetComponent<PlayerUIManager>();
        teleport = GameObject.FindGameObjectsWithTag("Teleport");

        // function to increase difficulty for players
        if(singlePlayer)
        {
            IncreaseDifficulty();
        }
    }
    public virtual void IncreaseDifficulty() // increase the difficulty of the game in single player
    {
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
            player[i].transform.position = teleport[i].transform.position; // set position for player in minigame
            player[i].GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static; // prevent movement until necessary
        }
    }
    public void TutorialUI()
    {
        // nothing here, Yahir, this is your spot 
    }
    public IEnumerator StartGame(bool enable) // teleports players into minigame
    {
        TutorialUI();
        TeleportPlayers(); // teleport players into the game
        yield return new WaitForSeconds(3); // Temporary during debug for minigames

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
        //setting up the game score length
        var scoreArray = new int[player.Length];
        gameScore = scoreArray;
 
        yield return new WaitForSeconds(3); // Temporary during debug for minigames
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
        yield return new WaitForSeconds(3); // Temporary during debug for minigames
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
    public void TeleportBack() // bring players back to their spawn point
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraControl>();
        cam.TeleportCamera(camPos, fov); // change camera into minigame spot

        player = plrManage.player;
        teleport = GameObject.FindGameObjectsWithTag("Teleport"); // check if null, replace spawns

        for (int i = 0; i < player.Length; i++) // for loop to set all players in correct position for game
        {
            if(!singlePlayer)
            {
                var playerStat = player[i].GetComponent<PlayerStats>(); // calling player's position
                var movementManager = GameObject.FindGameObjectWithTag("Movement Manager").GetComponent<MovementManager>();

                // moving player back to tile
                var tile = movementManager.CallTile(playerStat.position);
                player[i].transform.position = tile.transform.position; // set position for player in board
                player[i].GetComponent<PlayerMovement>().GameSwitch(false);


            }
            else
            {
                player[i].transform.position = new Vector2(spSpawn.position.x,spSpawn.position.y); // bring player back to map
                player[i].GetComponent<PlayerMovement>().GameSwitch(false);
                cam.TeleportCamera(spSpawn, 20); // set position of camera 
                
            }
        }
    }
    public virtual IEnumerator EndGame()
    {
        TeleportBack();
        Debug.Log("Game has ended.");

        if(singlePlayer)
        {
            //increase level
            spManage.IncreaseLevel();
        }

        Destroy(gameObject, 1f);
        yield return null;
    }
    public virtual IEnumerator EndGame(bool won) // single player endgame routine
    {
        if(won)
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
        yield return null;
    }
    public virtual IEnumerator EndGame(int winner) // coroutine to end the game as a player has won.
    {
        yield return new WaitForSeconds(2); 
        for(int i = 0; i < player.Length; i++)
        {
            var playerStat = player[i].GetComponent<PlayerStats>();
            if(playerStat.turnOrder == winner)
            {
                playerStat.wins++;
            }
        }
        Debug.Log("Game has ended.");
        StartCoroutine(EndGame());

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
        StartCoroutine(EndGame());
        yield return null;
    }
}
