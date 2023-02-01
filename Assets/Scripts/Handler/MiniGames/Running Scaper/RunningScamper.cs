/*
 * Purpose:
 * 
 * @author Yahir Bonilla
 * @version January 30, 2023
 * @os Win 11
 * @editor VS-Code
 *
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunningScamper : GameHandler
{
    [Header("Minigame Parameters")]
    public int timerLength = 10;
    public float playerBaseSpeed = 2;
    public float playerMovementIncrement = 1.1f;
    public float playerIncrementCooldown = 0.5f;
    public int playerJumpPower = 5;
    public float gridMapDisappearSpeed = 2f;

    public float timerForGridMaps = 8f;

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

    [SerializeField]
    protected List<GameObject> cagedPlayers;

    bool isAllLosers = false;

    
    // Start is called before the first frame update
    void Start()
    {
        allowCameraFollow = true;
        //Call to teleport player's to their positions
        StartCoroutine(StartGame(false, false, false));
        //Find all grid map spawn points
        FindSpawnPoints();
        StartCoroutine(StartGame());
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

    public void addCagedPlayer(GameObject playerObj){
        cagedPlayers.Add(playerObj);
        isAllLosers = checkLosers();
        if(isAllLosers){
            timerEnd();
        }
    }

    bool checkLosers(){
        //Check if caged players is zero
        if(cagedPlayers.Count > 0){
            int playersCaged = 0;
            for(int i = 0; i < cagedPlayers.Count; i++){
                for(int b = 0; b < player.Length; b++){
                    if(cagedPlayers[i] == player[b]){
                        playersCaged++;
                    }
                }
            }

            //Check if the amount of caged players is the same as the player list
            if(playersCaged == player.Length){
                return true;
            }else{
                //Debug.LogError("Missing player or player not added properly to player lists");
                return false;
            }
        }else{
            Debug.LogWarning("No caged players yet!");
            return false;
        }
    }

    bool spawnMap(){
        if (gridMapSpawnPoints.Length > 0)
        {
            var multiplier = timerForGridMaps;
            for (int i = 0; i < gridMapSpawnPoints.Length; i++)
            {
                int randM = Random.Range(0, gridMaps.Length);
                GameObject map = Instantiate(gridMaps[randM], gridMapSpawnPoints[i].transform.position, gridMapSpawnPoints[i].transform.rotation, gridMapSpawnPoints[i].transform);
                Scamper_GridMap scam = map.GetComponent<Scamper_GridMap>();
                if(scam != null){
                    scam.speed = gridMapDisappearSpeed;
                    scam.timer += multiplier;
                    multiplier += timerForGridMaps;
                }
            }
            return true;
        }else{
            Debug.LogError("No grid map spawn points found");
            return false;
        }
    }

    IEnumerator StartGame()
    {
        yield return new WaitForSeconds(miniGameStartUpTime);
        //Call main game methods
        //Set default map array to use
        gridMaps = setMapArray();
        if(gridMaps == null)
            Debug.LogError("No players were found!");
            StopCoroutine(StartGame());

        if (!setStaticDir())
            Debug.LogError("Players not found");

        //Spawn the grid maps randomly
        spawnMap();

        //Game timer
        if (_timer)
        {
            //Wait to call method according to the length of timerLength in seconds
            Invoke("timerEnd", timerLength);
        }

        StopCoroutine(StartGame());
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
                player[i].GetComponent<PlayerMovement>().setStaticDir(playerDir, playerBaseSpeed,playerJumpPower, playerMovementIncrement, playerIncrementCooldown);
                Debug.Log(player[i].name + "'s movement was set to static movement");
            }
            return true;
        }

        return false;
    }

    int getWinner(){
        int coin = checkScore();

        int dist = checkDistanceScore();

        Debug.Log(player[dist].name + "" + (dist) + " got top in score");
        Debug.Log(player[coin].name + "" + coin + " got the farthest");

        //Decide the winner by score and how far they went
        if(dist > coin || dist < coin){ //Check if distance is a different player from coin then make the player with the highest coin win 
            return coin;
        }else if(dist == coin){ //Check if the one with the highest distance and got the most coin is the same player
            return dist;
        }

        return -1;
    }

    //This can also be called as the end game check
    void timerEnd()
    {
        //Debug
        Debug.Log("Timer ended");
        
        if(isAllLosers){
            StartCoroutine(EndGame(5));
            Debug.Log("All players are losers!");
            return;
        }

        winner = getWinner();

        if(winner < 0){
            Debug.LogError("Couldn't decide winner!");
            return;
        }

        //set camera follow to false
        allowCameraFollow = false;

        //Call game end
        Debug.Log("Winner is player " + winner);
        
        //loop through all players to set their gameswitch to all false
        for(int i = 0; i < player.Length; i++){
            player[i].GetComponent<PlayerMovement>().GameSwitch(false);
        }

        StartCoroutine(EndGame(winner));
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
