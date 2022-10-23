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

    public int playerCount;
    //for determining turns
 
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
        TurnGeneration();
    }
    private void TurnGeneration()
    {
        playerCount = playerManager.GetComponent<PlayerManager>().playerCount;
        int[] turnOrder = new int[4];

        //turnOrder[0] = Random.Range(1, playerCount);
        
        //running loop to decide order; plan is to check all positions to see if they have same number, and if they do, they reroll
        for (int count = 0; count <= playerCount; count++)
        {

            int i = Random.Range(1, playerCount+1);
            //VERY INEFFECTIVE STATEMENT, WILL FIX LATER
            while (i == turnOrder[0] || i == turnOrder[1])
            {
                //reroll statement to get new number
                i = Random.Range(1, playerCount + 1);
                
            }
            turnOrder[count] = i;
        }
        //checker statement, will be removed.
        for (int x = 0; x >= playerCount; x++)
        {
            print(x + " Turn is " + turnOrder[x]);
        }
    }

}
