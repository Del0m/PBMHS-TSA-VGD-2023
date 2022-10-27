/*armindelmo turnmanager.cs
 * the purpose of this program is to seperate all the users into their respective turns.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    private PlayerManager playerManager;
    public GameObject[] players;

    public int currentTurn;

    // Start is called before the first frame update
    void Start()
    {
        playerManager = GameObject.FindGameObjectWithTag("Player Manager").GetComponent<PlayerManager>();
        print(playerManager);

        
    }

    // Update is called once per frame
    void Update()
    {
        //TurnLocker(); // not needed
        
    }
    public void ChangeTurn() // to be called from player controls to change to the next turn
    {
        currentTurn = currentTurn + 1; // update to next turn
        if(currentTurn <= playerManager.playerCount) // set turn order to beginning
        {
            currentTurn = 1;
        }
    }

    public void GetPlayers() // grab players from PlayerManager and put into array
    {
        players = playerManager.players; // PlayerManager array
    }

}
