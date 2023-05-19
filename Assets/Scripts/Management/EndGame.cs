// armin delmo; endgame.cs; this will end the game for the multiplyer section of VGD
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EndGame : MonoBehaviour
{
    PlayerManager playerManager;


    public Transform camPosition;
    public Transform[] playerPosition; // 4 player slots

    public GameObject endScreen; // shows button to player, to return to menu
    public void Start() // find the player manager
    {
        playerManager = GameObject.FindGameObjectWithTag("Player Manager").GetComponent<PlayerManager>();
    }
    public IEnumerator EndRoutine()
    {
        var plr = new List<GameObject>();
        plr = EndSort(playerManager.player);

        EndMove(plr); // move the players to the map
        EndMove(GameObject.FindGameObjectWithTag("MainCamera")); // move camera to location

        // wait to show end screen to return to menu
        yield return new WaitForSeconds(5f);
        endScreen.SetActive(true);
    }
    public void EndMove(GameObject obj)
    {
        obj.transform.position = camPosition.position;
        if(obj.CompareTag("MainCamera"))
        {
            var camControl = obj.GetComponent<CameraControl>();
            camControl.TeleportCamera(camPosition.position, 20); // move the camera to the winning spot
            camControl.forgetDestination(); // prevent camera from moving after the game
        }
    }

    public void EndMove(List<GameObject> plr) // move all players to 
    {
        for (int i = 0; i < plr.Count; i++)
        {
            plr[i].transform.position = playerPosition[i].position; // moving player to position

            // setting player static
            plr[i].GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        }
        
    }
    public List<GameObject> EndSort(List<GameObject> plr)
    {
        var winOrdered = new List<GameObject>();
        // player object, amount of wins
        var winDictionary = new Dictionary<GameObject, int>(); // initalizing the dictinary that sorts players

        // for loop to add elements into the dictionary
        for(int i = 0; i < plr.Count; i++)
        {
            winDictionary.Add(plr[i], plr[i].GetComponent<PlayerStats>().wins); // adding player object, and their wins
        }
        // sorting the dictionary
        foreach (KeyValuePair<GameObject, int> player in winDictionary.OrderByDescending(key => key.Value))
        {
            // adding players into a list that will be returned
            winOrdered.Add(player.Key);
        }
        return winOrdered; // giving the sorted array to the player
    }
}
