using System;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    //Array of 'PlayerScore' class
    public PlayerScore[] playerS;

    GameObject[] playerObj;

    //Finds player's with 'PlayerScore' Script
    public void findPlayers()
    {
        //Finds Game Objects with tag 'Player'
        playerObj = GameObject.FindGameObjectsWithTag("Player");
        print("Player obj list is " + playerObj.Length);

        //Sort 1D array in order by name from least to greatest
        Array.Sort(playerObj, (a, b) => a.name.CompareTo(b.name));

        playerS = new PlayerScore[playerObj.Length];

        //Check every Object found with tag if they have 'PlayerScore.cs'
        for (int i = 0; i < playerObj.Length;)
        {
            if(i == playerObj.Length)
            {
                Debug.LogWarning("Player Score is done");
                return;
            }

            playerS[i] = playerObj[i].GetComponent<PlayerScore>();

            if (playerS[i] == null)
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
            i++;
        }
    }

    // Awake is called before first frame
    private void Awake()
    {
        this.gameObject.tag = "Score Manager";
    }
    void Start()
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
