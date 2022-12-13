//armin delmo, 11/13/22. Purpose of script is to handle minigame startup, grabbing players, starting game, etc.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using UnityEngine.VFX;

public class GameHandler : MonoBehaviour
{
    //player array to modify rigidbodies and teleport
    public GameObject[] player;

    //array of spawns
    public GameObject[] teleport;

    private void Start()
    {
        //grab teleports.
        teleport = GameObject.FindGameObjectsWithTag("Teleport");
    }
    public IEnumerator TeleportPlayers() // teleports players into minigame
    {
        Debug.Log("Waiting...");
        yield return new WaitForSeconds(5);

        player = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < player.Length; i++) // for loop to spawn players
        {
            Debug.Log("Teleporting and enabling player " + i);

            player[i].transform.position = teleport[i].transform.position; // set position for player in minigame
            player[i].GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic; //turn on player movement by allowing rigidbody to take movement

            yield return new WaitForSeconds(1); // wait for each player to be fully teleported in.
        }

    }
}
