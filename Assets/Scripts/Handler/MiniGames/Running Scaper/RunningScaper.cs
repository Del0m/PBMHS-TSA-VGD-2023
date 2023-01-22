/*
 * Purpose:
 * 
 * @author Yahir Bonilla
 * @version January 19, 2023
 * @os Arch Linux
 * @editor VS 2022
 *
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunningScaper : GameHandler
{
    [Header("Minigame Parameters")]
    public int timerLength = 10;
    public float playerBaseSpeed = 2;
    public float playerMovementIncrement = 1.1f;
    public float playerIncrementCooldown = 0.5f;
    public int playerJumpPower = 5;
    public double jumpCooldown = 1.5;

    [Header("Minigame Map params")]

    [Header("Minigame 4 Player Prefabs")]
    public GameObject[] mapPrefabs_4; //4 player grid maps

    [Header("Minigame 3 Player Prefabs")]
    public GameObject[] mapPrefabs_3; //3 player grid maps

    [Header("Minigame 2 Player Prefabs")]
    public GameObject[] mapPrefabs_2; //2 player grid maps

    [Header("Minigame 1 Player Prefabs")]
    public GameObject[] mapPrefabs_1; //1 player grid maps


    [Header("DEBUG")]
    [SerializeField]
    private bool _timer = false; //used for debug
    [SerializeField]
    private float miniGameStartUpTime = 4f;
    [SerializeField]
    private bool playerDir = true;
    [SerializeField]
    protected GameObject[] gridMapSpawnPoints;
    [SerializeField]
    private GameObject[] gridMaps;
    
    // Start is called before the first frame update
    void Start()
    {
        //Call to teleport player's to their positions
        StartCoroutine(TeleportPlayers(false, false, false));
        StartCoroutine(StartGame(miniGameStartUpTime));
    }

    GameObject[] setMapArray(){
        //Checks the amount of players and return a grid map array
        switch(player.Length){
            case 0:
                return null;
            case 1:
                return mapPrefabs_1;
            case 2:
                return mapPrefabs_2;
            case 3:
                return mapPrefabs_3;
            case 4:
                return mapPrefabs_4;
        }

        return null;
    }

    IEnumerator StartGame(float delay)
    {
        yield return new WaitForSeconds(delay);
        //Call main game methods
        //Find all grid map spawn points
        FindSpawnPoints();
        //Set default map array to use
        gridMaps = setMapArray();
        if(gridMaps == null)
            Debug.LogError("No players were found!");
            yield return null;

        yield return new WaitForSeconds(1.5f);
        //Spawn the grid maps randomly
        if (gridMapSpawnPoints.Length > 0)
        {
            for (int i = 0; i < gridMapSpawnPoints.Length; i++)
            {
                int randM = Random.Range(0, gridMaps.Length);
                GameObject map = Instantiate(gridMaps[randM], gridMapSpawnPoints[i].transform.position, gridMapSpawnPoints[i].transform.rotation);
            }
        }else{
            Debug.LogError("No grid map spawn points found");
            yield return null;
        }

        if (!setStaticDir())
            Debug.LogError("Players not found");

        //Game timer
        if (_timer)
        {
            //Wait to call method according to the length of timerLength in seconds
            Invoke("timerEnd", timerLength);
        }

        yield return null;
    }
    bool FindSpawnPoints()
    {
        gridMapSpawnPoints = GameObject.FindGameObjectsWithTag("Minigame Element");

        if(gridMapSpawnPoints.Length > 0)
        {
            Debug.Log("Found at least more than one spawn point for grid maps, total grid maps: " + gridMapSpawnPoints.Length);
            return true;
        }

        Debug.LogError("No grid map spawn points found!");
        return false;
    }

    bool setStaticDir()
    {
        //Loop through every player and set static direction
        
        //Protection condition, in case player list is empty
        if(player.Length > 0)
        {
            for (int i = 0; i < player.Length; i++)
            {
                player[i].GetComponent<PlayerMovement>().setStaticDir(playerDir, playerBaseSpeed,playerJumpPower, playerMovementIncrement, playerIncrementCooldown, jumpCooldown);
                Debug.Log(player[i].name + "'s movement was set to static movement");
            }
            return true;
        }

        return false;
    }

    void timerEnd()
    {
        //Debug
        Debug.Log("Timer ended");
        //Decide the winner by score and how far they went

        if(player.Length > 0)
        {
            int i = checkScore();

            Debug.Log(player[i].name + "" + (i) + " got top in score");

            i = checkDistanceScore();

            Debug.Log(player[i].name + "" + i + " got the farthest");
        }

        //Call game end
        //EndGame();
    }

    int checkScore()
    {
        //The first player automatically becomes of the highest score
        int playerIndex = 0;

        //Set the first player as the highest score
        int highestScore = gameScore[0];

        for(int i = 0; i < gameScore.Length; i++)
        {
            //Compare scores

            //compare using if conditions and replace the highest score accordingly
            if (gameScore[i] > highestScore)
            {
                highestScore = gameScore[i];
                playerIndex = i;
            }

        }

        return playerIndex;
    }

    //Make sure all players spawn at the same time 
    int checkDistanceScore()
    {
        //The first player automatically becomes of the highest score
        int playerIndex = 0;

        float xDistance = player[0].transform.position.x;

        for(int i = 0; i< player.Length; i++)
        {
            //Compare distance

            //Replace highest xDistance accordingly
            if (player[i].transform.position.x > xDistance)
            {
                xDistance = player[i].transform.position.x;
                playerIndex = i;
            }
        }

        return playerIndex;
    }


    public void ScorePlayer(int player, int scoreAmount) // to be accessed by prefab to increase score based on player action
    {
        gameScore[player] += scoreAmount;
    }

}
