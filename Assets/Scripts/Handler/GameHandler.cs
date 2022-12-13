//armin delmo, 11/13/22. Purpose of script is to handle minigame startup, grabbing players, starting game, etc.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class GameHandler : MonoBehaviour
{
    //player input manager to instantiate new player prefabs
    private PlayerInputManager spawnManager;
    private GameObject[] playerCount;

    public GameObject plafrab; // player prefab
    private void Start()
    {
        playerCount = GameObject.FindGameObjectsWithTag("Player");
    }
    public IEnumerator SpawnPlayers() // spawns players that will do movement.
    {
        for(int i = 0; i < playerCount.Length; i++) // for loop to spawn players
        {
            Debug.Log("Instantiating player " + i);
            //paireddevice is the device the player is currently using
            var instantiation = PlayerInput.Instantiate(plafrab, i, null, -1, playerCount[i].GetComponent<PlayerInput>().user.pairedDevices[0]);
            //spawnManager.JoinPlayer(i, -1, null, player[i].GetComponent<PlayerInput>().user.pairedDevices[0]); // spawns player with select prefab

            yield return new WaitForSeconds(1); // wait for each player to be fully spawned in.
        }

    }
}
