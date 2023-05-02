/*armindelmo MiniGameManager.cs
this script will load once all players have finished their turns, it will begin a new round and start a minigame for the players to participate in.

*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

public class MiniGameManager : MonoBehaviour
{
    //players
    private List<GameObject> players; // player array to change controls, bring to map, etc.

    //variables for minigame
    public bool hasStarted = false;

    //array for all available minigames
    public GameObject[] minigame;

    //randomizer to randomly select minigames
    private Randomizer rand;

    //defining the TurnManager
    //public TurnManager turnScript;

    public GameObject lastMinigame; // to prevent same games from playing one after another

    private void Awake() // to set tag so other manager can collect on Start()
    {
        this.gameObject.tag = "Mini Game Manager";
    }

    private void Start() // collect TurnManager
    {
    }
    private void SpawnMinigame()
    {
        // check if no minigames are found
        if(minigame.Length < 1) // recursively call the game after finding all of them in the map.
        {
            Debug.LogError("No minigames found, please initalize");
            // finding minigames
            minigame = GameObject.FindGameObjectsWithTag("Minigame");

            // recursively calling and returning.
            SpawnMinigame();
            return;
        }
        FindMinigame();
    }
    private void FindMinigame() // find minigame, check to see if it repeated
    {
        var rand = Random.Range(0, minigame.Length);

        if (minigame[rand] == lastMinigame) // check to see if game was played already
        {
            // recursion to end the function and look for one again

            Debug.LogWarning("Same minigame rolled, recursing.");

            FindMinigame();
            return;
        }

        var gameData = minigame[rand];
        var minigameInstance = Instantiate(gameData); // spawns minigame
        minigameInstance.SetActive(true); // make minigame exist in-game

        // add this minigame to the last minigame played
        lastMinigame = gameData;
        return;
    }

    public void MinigameStartup()
    {
        Debug.Log("running coroutine StartMiniGame");
        StartCoroutine(StartMiniGame());
    }
    public IEnumerator StartMiniGame()
    {
        /*
         * add ui updates here notifying players of upcoming minigame
         */


        yield return new WaitForSeconds(5); // wait 5 seconds before putting player into game.
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
