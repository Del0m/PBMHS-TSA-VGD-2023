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
    private GameObject[] minigame;

    //randomizer to randomly select minigames
    private Randomizer rand;

    private void Awake()
    {
        this.gameObject.tag = "Mini Game Manager";
    }

    private void SpawnMinigame() // randomly selects a minigame from public gameobject list
    {
        try // statement to check for invalid minigame length
        {
            if(minigame.Length < 0 )
            {
                //custom exception for debugging purposes.
                throw new System.Exception("Error found, no minigames located. Please add minigames through the public list.");
            }
            else
            {
                var pick = rand.DiceRoll(0, minigame.Length); // randomly selects a minigame using the length of minigame array
                Instantiate(minigame[pick]); // spawns minigame
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError(e);
        }
        

    }
    public IEnumerator StartMiniGame()
    {
        /*
         * add ui updates here notifying players of upcoming minigame
         */


        yield return new WaitForSeconds(5); // wait 5 seconds before putting player into game.
        SpawnMinigame();
    }
    public void EndMiniGame() //kills all minigames, brings back controls to players
    {
        hasStarted = false;
        Debug.Log("Ending minigame");
        players = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < players.Length; i++) // brings plrs back to board.
        {
            if(players.Length == 0)
            {
                break;
            }
            Debug.Log("Changing Board on Player " + (i + 1));
            players[i].GetComponent<PlayerInput>().SwitchCurrentActionMap("boardGamePlay");
        }

        KillGamesFromFile(); // deletes minigame
    }
    private void LoadGamesFromFile() // resource.load all games, pick one, deload
    { 
    }
    private void KillGamesFromFile()
    {
        var gameOut = GameObject.FindGameObjectWithTag("Minigame");
        Destroy(gameOut);
    }


}
