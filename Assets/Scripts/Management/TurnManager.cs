/*armindelmo turnmanager.cs
 * the purpose of this program is to seperate all the users into their respective turns.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    private PlayerManager playerManager;
    private GameObject[] players;

    public int currentTurn = 1;

    //minigame Management elements

    private MiniGameManager miniGameScript;

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.tag = "Turn Manager";//change this object to have turn manager tag

        playerManager = GameObject.FindGameObjectWithTag("Player Manager").GetComponent<PlayerManager>(); // call player manager for player array
        miniGameScript = GameObject.FindGameObjectWithTag("Mini Game Manager").GetComponent<MiniGameManager>(); // call manager to start / end / bring players to games.

    }
    private void Update()
    {
        NewRound();
    }
    public void ChangeTurn() // to be called from player controls to change to the next turn
    {
        currentTurn = currentTurn + 1; // update to next turn

    }
    public void NewRound() // if current turn > player.length; run minigame
    {
        while(currentTurn > players.Length)
        {
            currentTurn = 1;
            for (int i = 0; i == players.Length; i++)
            {
                //check all player turns
                players[i].GetComponent<PlayerControls>().PlayerTurn();
            }
            if (miniGameScript.hasStarted == false)
            {
                Debug.Log("Resetting turn!");
                currentTurn = 1;
                miniGameScript.StartMiniGame(players);
                miniGameScript.hasStarted = true;
            }
        }
        
    }

    public void GetPlayers(GameObject[] playerArray) // grab players from PlayerManager and put into array
    {
        players = playerArray; // grab array given from player Manager
        miniGameScript.GetPlayers(playerArray);
    }
    // on second thought, this is a very secure solution... Wont be used tho :-)
}
