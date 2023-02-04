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

    public Transform Destination;

    public CameraControl cam;
    [Header("Button")]
    public GameObject endButton; 

    //Scripts to get
    public PlayerManager pm;

    private GameObject[] players;

    private PlayerStats[] plStats;

    Dictionary<GameObject, int> map = new Dictionary<GameObject, int>();
        //List
    List<int> wins = new List<int>();

    public void End(){ //To be called by turn manager
        //Sort players by from highest win to lowest

        cam.setCamUpdate(false);
        cam.forgetDestination();

        cam.TeleportCamera(Destination, 20);

        players = pm.player;

        plStats = new PlayerStats[players.Length];

        //Get players from every player
        for(int i = 0; i < players.Length; i++){
            plStats[i] = players[i].GetComponent<PlayerStats>();
        }

        //Loop to add keys

        for(int i = 0; i < plStats.Length; i++){
            wins.Add(plStats[i].wins);
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

        int dex = 0;
        foreach(var i in map){
            pl[dex] = i.Key;
            dex++;
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

        // button
        endButton.SetActive(true);
    }

    IEnumerator restartGame()
    {
        yield return new WaitForSeconds(6f);
        SceneManager.LoadScene("StartMenu");
    }

    public void RestartGameBru(){
        StartCoroutine(restartGame());
    }
}


