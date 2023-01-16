/*
 * Purpose:
 * 
 * @author Yahir Bonilla
 * @version January 14, 2023
 * @os Win11
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

    [Header("DEBUG")]
    [SerializeField]
    private bool _timer = false; //used for debug
    [SerializeField]
    private float miniGameStartUpTime = 4f;
    [SerializeField]
    private bool playerDir = true;

    // Start is called before the first frame update
    void Start()
    {
        //Call to teleport player's to their positions
        StartCoroutine(TeleportPlayers(false, true, false));
        StartCoroutine(StartGame(miniGameStartUpTime));
    }

    IEnumerator StartGame(float delay)
    {
        yield return new WaitForSeconds(delay);
        //Call main game methods
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

        int i = checkScore();

        Debug.Log(player[i].name + "" + (i) + " got top in score");

        i = checkDistanceScore();

        Debug.Log(player[i].name + "" + i + " got the farthest");


        //Call game end
        EndGame();
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
