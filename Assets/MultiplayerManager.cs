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

    public GameObject[] players;
    public Transform[] spawns;

    private List<PlayerStats> ps;

    // Start is called before the first frame update
    void Start()
    {
        //Player Manager MUST exist in scene
        pm = GameObject.FindGameObjectWithTag("Player Manager").GetComponent<PlayerManager>();

        StartCoroutine(startTurns());        
    }

    IEnumerator startTurns(){
        yield return new WaitForSeconds (timeToStart);
        //Copy player list
        players = pm.player;
        //Copy spawn list
        spawns = pm.spawn;

        //teleport players
        if((players.Length != 0 && players.Length > 0) && (spawns.Length != 0 && spawns.Length > 0)){
            for(int i = 0; i < players.Length; i++){
                players[i].transform.position = spawns[i].position;
            }
        }else{
            Debug.LogError("Player or Spawn list from player manager came empty!");
            StopAllCoroutines();
        }

        yield return new WaitForSeconds(1); //debug
        //Start turn order

        //arrange players score script to local list
        for(int i = 0; i < players.Length; i++){
            ps.Add(players[i].GetComponent<PlayerStats>());
        }   
    }
}
