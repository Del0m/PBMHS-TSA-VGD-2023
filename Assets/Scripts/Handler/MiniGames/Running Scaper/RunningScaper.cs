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
    public float timerLength = 10f;

    [Header("DEBUG")]
    [SerializeField]
    private bool _timer = false; //used for debug

    // Start is called before the first frame update
    void Start()
    {
        //Call to teleport player's to their positions
        StartCoroutine(TeleportPlayers(false, true, true));
        StartCoroutine(StartGame(4f));
    }

    IEnumerator StartGame(float delay)
    {
        yield return new WaitForSeconds(delay);
        //Call main game methods

        //Game timer
        if (_timer)
        {
            //Wait to call method according to the length of timerLength in seconds
            Invoke("timerEnd", timerLength);
        }

        yield return null;
    }

    void timerEnd()
    {
        //Call game end
        EndGame();
    }


    public void ScorePlayer(int player, int scoreAmount) // to be accessed by prefab to increase score based on player action
    {
        gameScore[player] += scoreAmount;
    }

}
