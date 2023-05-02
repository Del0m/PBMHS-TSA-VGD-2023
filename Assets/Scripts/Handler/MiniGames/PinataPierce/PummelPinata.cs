// armin delmo; 1/15/23
// the purpose of this program is to facilitate and score for the players when they kill the pinata
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class PummelPinata : GameHandler
{
    [Header("Pummel the Pinata Objects")]
    public GameObject pinata; // pinata players will fight in order to win the game
    public GameObject pinataPrefab;
    public GameObject pinataSpawn;

    public GameObject killer;

    [Header("Single Player Variables")]
    public int health;
    public int time; 

    private void Start()
    {
        uiManager = GameObject.FindGameObjectWithTag("PlayerUIManager").GetComponent<PlayerUIManager>();
        StartCoroutine(StartGame()); // teleport to game

        StartCoroutine(SpawnObject());
    }
    public override void IncreaseDifficulty() // make pinata harder to kill
    {
        base.IncreaseDifficulty();
        health = (int)(health * multiplier);
        minimumToWin = 10; // ensures the player will always lose if they don't kill the pinata in time.
    }
    public override IEnumerator StartGame()
    {
        StartCoroutine(base.StartGame());
        yield return new WaitForSeconds(3); // wait for StartGame() to finish loading

        // run loop to move players
        for (int i = 0; i < player.Count; i++)
        {
            var playerMovement = player[i].GetComponent<PlayerMovement>();

            playerMovement.GameSwitch(true);
        }
        yield return null;
    }
    public override IEnumerator PreGameRoutine() // adding a timer to the minigame in singleplayer
    {
        IncreaseDifficulty();
        yield return StartCoroutine(base.PreGameRoutine());
        yield return new WaitForSeconds(3);
        // starting the timer
        StartCoroutine(gameUI.Timer(time));

    }
    IEnumerator SpawnObject() // spawn objects for the game to begin
    {
        yield return new WaitForSeconds(2f);
        try
        {
            pinataPrefab = Instantiate(pinata, pinataSpawn.transform.position, new Quaternion(0, 0, 0, 0));

            pinataPrefab.GetComponent<PinataObject>().minigame = this;
        }
        catch (System.Exception)
        {
            Debug.Log("Pinata prefab isn't assigned!");
            throw;
        }
        yield return null;
    }
}
