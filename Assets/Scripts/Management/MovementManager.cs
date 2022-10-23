/* armindelmo MovementManager.cs 10 / 2 / 22
  The purpose of this program is to move the player from one tile to another.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementManager : MonoBehaviour
{
    //initalizing
    public Transform[] tiles;
    GameObject player;
    int position;
    public int movement;
    void Start()
    {
        player = this.gameObject;
        position = 1;
        int movement = 0;
        MoveTile(player, tiles, position, movement);
    }

    private void MoveTile(GameObject player, Transform[] tiles, int position, int movement)
    {
        Debug.Log("moving object to position " + position);
        //will change position, then change transform of player to tile.
        position = movement + position - 1;
        
        //change position to new tile
        player.transform.position = new Vector3(tiles[position].position.x, tiles[position].position.y, player.transform.position.z); // no z
    }
}
