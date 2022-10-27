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

    private void TurnLocker()
    {
        //locks other turns to only allow one person to move
        
        for (int i = 0; i < players.Length; i++)
        {
            //disabling and enabling input of controllers
            
        }


    }

    public void GetPlayers() // grab players from PlayerManager and put into array
    {
        players = playerManager.players; // PlayerManager array
    }

}
