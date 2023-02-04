using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour
{
    [Header("Params")]
    public Transform[] playerPositions;

    //Scripts to get
    public PlayerManager pm;

    private GameObject[] players;

    private PlayerStats[] plStats = new PlayerStats[4]; 

    void Start(){
        if(pm == null){
            pm = GameObject.FindGameObjectWithTag("Player Manager").GetComponent<PlayerManager>();
            players = pm.player;

            int index = 0;
            //Get players from every player
            foreach(GameObject player in players){
                plStats[index] = player.GetComponent<PlayerStats>();
                index++;
            }
        }
    }

    public void End(){ //To be called by turn manager
        //Sort players by from highest win to lowest
        
        Dictionary<GameObject, int> map = new Dictionary<GameObject, int>();
        //List
        List<int> wins = new List<int>();

        //Loop to add keys
        foreach(var i in plStats){
            wins.Add(i.wins);
        }

        //Sort it  
        wins.Sort();
        //Reverse it
        wins.Reverse();

        for(int i = 0; i < players.Length; i++){
            map.Add(players[i], plStats[i].wins);
        }

        map.OrderBy(x => x.Value).ToList();

        //player index after here is the highest play
        GameObject[] pl = new GameObject[4];

        int index = 0;
        foreach(var i in map){
            pl[index] = i.Key;
            index++;
        }

        //Send the players to hell
        for(int i = 0; i < players.Length; i++){
            PlayerMovement p = players[i].GetComponent<PlayerMovement>();
            players[i].transform.position = playerPositions[i].position;
            if(p != null){
                //Game switch
                p.GameSwitch(false);
            }
        }

        StartCoroutine(restartGame());
    }

    IEnumerator restartGame()
    {
        yield return new WaitForSeconds(8f);
        SceneManager.LoadScene("StartMenu");
    }
}


