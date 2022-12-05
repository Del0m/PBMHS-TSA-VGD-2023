/* armindelmo MovementManager.cs 10 / 2 / 22
  The purpose of this program is to move the player from one tile to another.
*/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementManager : MonoBehaviour
{
    //tile and tile related goods
    public Transform[] tile;

    private TurnManager turnManage;

    //player array
    private GameObject[] players;

    private void Awake() // for manager initalization
    {
        this.gameObject.tag = "Movement Manager";
        /*plrManage = GameObject.FindGameObjectWithTag("Player Manager").GetComponent<PlayerManager>();
        turnManage = GameObject.FindGameObjectWithTag("Turn Manager").GetComponent<TurnManager>(); */
    }

    void Start()
    {
        tile = InitializeTiles();
    }
    public void GetPlayers(GameObject[] foreignPlayer) // to grab players from playermanager array
    {
        players = foreignPlayer;
    }

    Transform[] InitializeTiles()
    {
        //will grab all tiles on map, and will find length, and categorize them
        var tileAmount = GameObject.FindGameObjectsWithTag("Tile");
        var localTile = new Transform[tileAmount.Length];

        for(int i = 0; i < tileAmount.Length; i++) // loop through all tiles, put them in neat array :)
        {
            if (GameObject.Find(i.ToString()) == null)
            {
                Debug.LogError("Issue in finding tile " + i);
            }
            Debug.Log("collecting tile " + i);
           localTile[i] = GameObject.Find(i.ToString()).transform;
        }
        return localTile;
    }
    public Transform CallTile(int position, int increase)
    {
        var newPosition = position + increase; // adding their current position and rolled die

        //giving them the specific tile
        return tile[newPosition];
    }
}
