using System.Collections;
using UnityEngine;

public class Sokobot : GameHandler
{
    [Header("Sokobot Parameters")]
    public int amount; // sets how many holes and crates will appear on the map
    public int time; // time left to complete the game
    [Header("Sokobot Objects")]
    public GameObject crate;
    public GameObject hole;

    [Header("Sokobot Score")]
    public int holesLeft;
    private void Start()
    {
        StartCoroutine(StartGame()); // enable basic movement and topdown movement
        GameRoutine();
    }
    public override void IncreaseDifficulty() // make more boxes to drop in
    {
        base.IncreaseDifficulty();
        amount = (int)(amount * multiplier);

    }
    public override IEnumerator StartGame()
    {
        StartCoroutine(base.StartGame());

        // for loop to allow all players controls
        yield return new WaitForSeconds(3); // wait for the countdown to finish

        for (int i = 0; i < player.Count; i++)
        {
            var playerMovement = player[i].GetComponent<PlayerMovement>();

            playerMovement.GameSwitch(true,true);
        }
        yield return null;
    }
    public override IEnumerator PreGameRoutine()
    {
        yield return base.PreGameRoutine();

        yield return new WaitForSeconds(3);
        yield return StartCoroutine(gameUI.Timer(time)); // running the timer
    }

    public void ScoreHole(GameObject pusher) // runs whenever player pushes box into hole
    {
        if(pusher == null || pusher.GetComponent<PlayerStats>() == null){
            return;
        }
        // locate the pusher in the list
        var scoreAdd = player.IndexOf(pusher);
        Debug.Log(scoreAdd);
        Debug.Log(player[scoreAdd]);

        gameScore[scoreAdd]++;

        UpdateHole(); // check to see if the game should be done
    }
    public void UpdateHole() // runs, checks to see if there are no holes left.
    {
        if(holesLeft <= 0)
        {
            StartCoroutine(EndGame(CheckWinner()));
        }
        gameUI.ModifyText("Holes Left: " + holesLeft);  // change the amount of holes left to inform player
    }

    void GameRoutine() // runs the routine of the game, spawns holes where they need to be.
    {

        holesLeft = amount; // to track for single player and other factos

        for(int i = 0; i < amount; i++)
        {
            var crateInstance = Instantiate(crate, RandPosition(), new Quaternion(), this.gameObject.transform); // spawning crate for sokobot
            var holeInstance = Instantiate(hole, RandPosition(), new Quaternion(), this.gameObject.transform); // spanwing hole for sokobot

            holeInstance.GetComponent<SokobotHole>().minigame = this; // grab mother script
            crateInstance.GetComponent<SokobotCrate>().minigame = this; // grab mother script


        }
    }
}
