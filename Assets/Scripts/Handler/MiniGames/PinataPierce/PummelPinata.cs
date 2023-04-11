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
    public int time = 45; // always 45s

    private void Start()
    {
        uiManager = GameObject.FindGameObjectWithTag("PlayerUIManager").GetComponent<PlayerUIManager>();
        StartCoroutine(StartGame()); // teleport to game

        StartCoroutine(SpawnObject());
    }
    bool runningEnd; // prevent end routine being ran multiple times
    private void Update()
    {
        if(killer != null && !runningEnd) // end game when killer is found
        {
            runningEnd = true;
            var killerNum = killer.GetComponent<PlayerStats>().turnOrder;
            StartCoroutine(EndGame(killerNum));
            uiManager.ChangeUI(false, uiManager.timeLeftUI.gameObject);
        }
        if(uiManager.timesUp)
        {
            StartCoroutine(EndGame(CheckWinner())); // running end coroutine for singleplayer
        }
    }
    public override IEnumerator PreGameRoutine() // adding a timer to the minigame in singleplayer
    {
        yield return StartCoroutine(base.PreGameRoutine());
        // turning on the health bar
        uiManager.ChangeUI(true, uiManager.healthBarUI);

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
