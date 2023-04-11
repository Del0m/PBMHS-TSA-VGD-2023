using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerManager : MonoBehaviour
{
    //Checks all the players if they're ready

    //Take player list from playermanager
    //Teleport each player to their respected points
    //On start set the turnorder and when a player presses a button (On layer controls add callback for that button) start turns

    [Header("Debug")]
    public PlayerManager pm;

    public float timeToStart = 5f;

    // Start is called before the first frame update
    void Start()
    {
        //Player Manager MUST exist in scene
        pm = GameObject.FindGameObjectWithTag("Player Manager").GetComponent<PlayerManager>();

        StartCoroutine(StartTurns());        
    }

    IEnumerator StartTurns()
    {
        yield return new WaitForSeconds (timeToStart);
        var player = pm.player;

        //teleport players
        for(int i = 0; i < player.Count; i++)
        {
            player[i].transform.position = pm.spawn[i].transform.position;
        }
    }
}
