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

    private void Start()
    {
        StartCoroutine(TeleportPlayers(false, false, true)); // teleport to game

        StartCoroutine(SpawnObject());
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
    public override IEnumerator EndGame(int player)
    {
        var winner = killer.GetComponent<PlayerStats>();
        return base.EndGame(winner.turnOrder);
    }
}
