// armin delmo; 1/25/23; purpose of script is to move players into a losing zone after meeting a losing paramenter
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Losezone : MonoBehaviour
{
    private Rigidbody2D rb;
    public GameObject[] heldPlayer;
    public Transform[] lostSpot;

    public int spotOpen = 0;

    public ColorMatch minigame;

    private void Start() // grabbing rigid body
    {
        rb = GetComponent<Rigidbody2D>(); 
    }
    private void LateUpdate()
    {
        if(minigame.hasStopped != true){
            for(int i = 0; i < heldPlayer.Length; i++) // keep players in the loser spot
            {
                heldPlayer[i].transform.position = lostSpot[i].position;
            }
        }
    }
    void CheckLosers()
    {
        var playerArray = GameObject.FindGameObjectsWithTag("Player");
        if(playerArray.Length <= heldPlayer.Length)
        {
            // run loser function
            StartCoroutine(minigame.LoseGame());

        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player")) // check if drop is player
        {
            var rbPlayer = collision.gameObject.GetComponent<PlayerMovement>().rb;
            rbPlayer.bodyType = RigidbodyType2D.Static; // prevent any more movement

            // set player's stat to lost for the end of the game
            collision.GetComponent<PlayerStats>().lost = true;

            Array.Resize(ref heldPlayer, heldPlayer.Length + 1); // resize array to use in losing function
            heldPlayer[spotOpen] = collision.gameObject; // put in loser array to be moved in

            spotOpen++; // move to the next spot to allow a player to enter
            // run function to see if all players have lost
            CheckLosers(); 
        }
    }
}
