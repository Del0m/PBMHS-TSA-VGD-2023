using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    //Array of 'PlayerScore' class
    public PlayerScore[] playerS;

    //Finds player's with 'PlayerScore' Script
    void findPlayers()
    {
        //Finds Game Objects with tag 'Player'
        GameObject[] playerObj;
        playerObj = GameObject.FindGameObjectsWithTag("Player");

        //Check every Object found with tag if they have 'PlayerScore.cs'
        for (int i = 0; i < playerObj.Length; i++)
        {
            playerS[i] = playerObj[i].GetComponent<PlayerScore>();
            if(playerS[i] == null)
            {
                Debug.LogError(playerObj[i].name + " is missing PlayerScore.cs!");
                return;
            }
            else
            {
                playerS[i].playerIndex = i;
                //debug
                print(playerObj[i].name + " has an index of " + playerS[i].playerIndex);
            }
        }
    }

    // Awake is called before first frame
    void Awake()
    {
        findPlayers();
    }

    public void addScore(int amount, int index)
    {
        //Add score to a player in the list with 'PlayerScore.cs'
        if (playerS[index] != null)
        {
            playerS[index].Score += amount;
        }
        else
        {
            Debug.LogError("Player does not have PlayerScore.cs!");
            return;
        }
    }

    public void deductScore(int amount, int index)
    {
        //Deduct score to a player in the list with 'PlayerScore.cs'
        if (playerS[index] != null)
        {
            playerS[index].Score -= amount;
        }
        else
        {
            Debug.LogError("Player does not have PlayerScore.cs!");
            return;
        }
    }
    
}
