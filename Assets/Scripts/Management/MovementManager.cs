/* armindelmo MovementManager.cs 10 / 2 / 22
  The purpose of this program is to move the player from one tile to another.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementManager : MonoBehaviour
{
    //tile and tile related goods
    public Transform[] tiles;

    //managers
    private PlayerManager plrManage;
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
        tiles = InitializeTiles();
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
            Debug.Log("collecting tile " + i);
           localTile[i] = tileAmount[i].transform;
        }
        return localTile;
    }

}
