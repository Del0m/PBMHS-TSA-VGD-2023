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

    public int currentTurn;

    //minigame Management elements

    private MiniGameManager miniGameManger;

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.tag = "Turn Manager";//change this object to have turn manager tag

        playerManager = GameObject.FindGameObjectWithTag("Player Manager").GetComponent<PlayerManager>(); // call player manager for player array
        miniGameManger = GameObject.FindGameObjectWithTag("Mini Game Manager").GetComponent<MiniGameManager>(); // call manager to start / end / bring players to games.

    }
    public void ChangeTurn() // to be called from player controls to change to the next turn
    {
        Debug.Log(currentTurn);
        currentTurn = currentTurn + 1; // update to next turn
        Debug.Log("Changing turn! Turn is now: " + currentTurn);
        if(currentTurn > players.Length)
        {
            Debug.Log("Resetting turn!");
            currentTurn = 1;
        }
    }

    public void GetPlayers(GameObject[] playerArray) // grab players from PlayerManager and put into array
    {
        players = playerArray; // grab array given from player Manager
    }
    // on second thought, this is a very secure solution... Wont be used tho :-)
}
