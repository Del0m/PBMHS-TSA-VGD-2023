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
    // GameTime! UI 
    public GameObject ui;
    //array for all available minigames
    public GameObject[] minigame;

    public GameObject lastMinigame; // to prevent same games from playing one after another

    private void Awake() // to set tag so other manager can collect on Start()
    {
        this.gameObject.tag = "Mini Game Manager";
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
        StartCoroutine(StartMiniGame());
    }
    public IEnumerator StartMiniGame() // turn on minigame, let players know
    {
        ui.SetActive(true);
        yield return new WaitForSeconds(5); // wait 5 seconds before putting player into game.
        ui.SetActive(false);
        SpawnMinigame();
    }
}
