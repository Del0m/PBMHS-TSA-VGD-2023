// armin delmo; 1/6/22; the purpose of this script is to manage the mini game dubbed "hot potato" for lack of a better name
// it will manage the instantiation of the "hot potato" and calculate points for player moves done during the game

using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class HotPotato : GameHandler
{
    // variables for hot potato instantiation and time

    double time; // set here b/c will be instantiated through prefab and therefore unreliable
    
    public GameObject potatoObject; // WILL BE PREFAB

    private void Start()
    {
        StartCoroutine(TeleportPlayers(false, false, false));
        StartCoroutine(StartGame());
    }
    IEnumerator StartGame() // starts game by spawning the potato 
    {
        yield return new WaitForSeconds(4f);
        SpawnPotato();
        yield return null;
    }
    private void TimeToExplosion(int min, int max) // max is increased by one to allow person to not think of max exclusion bs
    {
        time = Random.Range(min,max+1);
        return;
    }
    private void SpawnPotato() // once players spawn, will spawn potato on random player and go from there 
    {
        // finding player variables to decide who gets the hot potato
        var player = GameObject.FindGameObjectsWithTag("Player");

        var playerCount = GameObject.FindGameObjectsWithTag("Player").Length;

        // randomly selecting player from 4
        var rand = Random.Range(0, player.Length);

        Debug.Log("Spawning Potato on player " + rand);
        var gamePotato = Instantiate(potatoObject, player[rand].transform.position, new Quaternion(0,0,0,0));

        // randomly select time left until potato explosion
        TimeToExplosion(10, 20);

        // set variables on instantiated "potato object"
        gamePotato.GetComponent<PotatoObject>().parentScript = this;
        gamePotato.GetComponent<PotatoObject>().timeLeft = time;
        gamePotato.GetComponent<PotatoObject>().holdingPlayer = rand;
    }
    public void ScorePlayer(int player, int scoreAmount) // to be accessed by prefab to increase based on player action
    {
        gameScore[player] += scoreAmount;
    }


}
