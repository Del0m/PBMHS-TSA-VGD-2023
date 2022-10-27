/*armindelmo turnmanager.cs
 * the purpose of this program is to seperate all the users into their respective turns.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    private GameObject playerManager;
    private PlayerManager manageScript;
    public GameObject[] players;

    // Start is called before the first frame update
    void Start()
    {
        //getting playerManager to call for functions l8r
        playerManager = GameObject.Find("Player Manager");
        print(playerManager);
        
    }

    // Update is called once per frame
    void Update()
    {
        TurnLocker();
        GrabPlayers();
    }
    private void GrabPlayers()
    {
        //grabs all players for script
        for (int i = 0; i < GameObject.FindGameObjectsWithTag("Player").Length; i++)
        {
            players = GameObject.FindGameObjectsWithTag("Player");
            print(players);
        }
    }
    private void TurnLocker()
    {
        //locks other turns to only allow one person to move
        
        for (int i = 0; i < players.Length; i++)
        {
            //disabling and enabling input of controllers
            
        }


    }

}
