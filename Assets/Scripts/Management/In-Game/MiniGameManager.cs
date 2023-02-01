/*armindelmo MiniGameManager.cs
this script will load once all players have finished their turns, it will begin a new round and start a minigame for the players to participate in.

*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MiniGameManager : MonoBehaviour
{
    //players
    private GameObject[] players; // player array to change controls, bring to map, etc.

    //variables for minigame
    public bool hasStarted = false;

    //array for all available minigames
    public GameObject[] minigame;

    //randomizer to randomly select minigames
    private Randomizer rand;

    //defining the TurnManager
    public TurnManager turnScript;

    private void Awake() // to set tag so other manager can collect on Start()
    {
        this.gameObject.tag = "Mini Game Manager";
    }

    private void Start() // collect TurnManager
    {
    }

    private void SpawnMinigame() // randomly selects a minigame from public gameobject list
    {
        try // statement to check for invalid minigame length
        {
            if(minigame.Length < 1 ) // 0 minigames
            {
                //custom exception for debugging purposes.
                throw new System.Exception("Error found, no minigames located. Please add minigames through the public list.");
            }
            else
            {
                var pick = Random.Range(0, minigame.Length); // randomly selects a minigame using the length of minigame array

                Debug.Log(minigame[pick].name);
                var minigameInstance = Instantiate(minigame[pick]); // spawns minigame

                minigameInstance.SetActive(true); // make minigame exist in-game
            }
        }
        catch (System.Exception)
        {
            Debug.LogError("Warning, no minigames found!");
        }
        

    }
    public void MinigameStartup()
    {
        Debug.Log("running coroutine StartMiniGame");
        StartCoroutine(StartMiniGame());
    }
    public IEnumerator StartMiniGame()
    {
        Debug.Log("Starting new minigame!");
        /*
         * add ui updates here notifying players of upcoming minigame
         */


        yield return new WaitForSeconds(2); // wait 5 seconds before putting player into game.
        SpawnMinigame();
    }
    /*public IEnumerator UIEndGame() // ui to show users the minigame is ending
    {
        //show UI
        yield return new WaitForSeconds(5); // wait 5 seconds before deleting board.
        EndMiniGame();
    }
    public void EndMiniGame() //kills all minigames, brings back controls to players
    {
        hasStarted = false;
        Debug.Log("Ending minigame");
        players = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < players.Length; i++) // brings plrs back to board.
        {
            if (players.Length == 0)
            {
                break;
            }
            Debug.Log("Returning player back to position.");
            //find corresponding tile, return them back to the position.
            players[i].transform.position = GameObject.Find(players[i].GetComponent<PlayerStats>().position.ToString()).transform.position;
        }
        KillGamesFromFile(); // deletes minigame

        turnScript.SetTurn(1);

        //elapse the turns; once reaches end amount, game will end.
        turnScript.roundsElapsed++;
    }
    */
    private void KillGamesFromFile() // remove minigame instance.
    {
        var gameOut = GameObject.FindGameObjectWithTag("Minigame"); // find object with "minigame" tag
        Destroy(gameOut);
    }


}
