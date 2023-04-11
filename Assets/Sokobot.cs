using JetBrains.Annotations;
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
    public override IEnumerator StartGame()
    {
        // for loop to allow all players controls, add this in the override 
        
        for (int i = 0; i < player.Count; i++)
        {
            var playerMovement = player[i].GetComponent<PlayerMovement>();

            playerMovement.GameSwitch(true,true);
        }
        return base.StartGame();
    }
    bool hasRan; // to prevent multiple runs
    private void Update()
    {
        if(uiManager.timesUp && !hasRan)
        {
            hasRan = true;
            StartCoroutine(EndGame(CheckWinner()));

        }
    }
    public override IEnumerator PreGameRoutine()
    {
        yield return base.PreGameRoutine();

        yield return new WaitForSeconds(3);
        yield return StartCoroutine(uiManager.UpdateClock(time)); // running the timer
    }

    public void ScoreHole(GameObject pusher) // runs whenever player pushes box into hole
    {
        if(pusher == null || pusher.GetComponent<PlayerStats>() == null){
            return;
        }
        var pusherOrder = pusher.GetComponent<PlayerStats>().turnOrder; // grabs their order, gives them score accordingly
        gameScore[pusherOrder]++; // scores player

        UpdateHole(); // check to see if the game should be done
    }
    public void UpdateHole() // runs, checks to see if there are no holes left.
    {
        if(holesLeft <= 0)
        {
            StartCoroutine(EndGame(CheckWinner()));
        }
    }
    Vector2 RandomizePosition() // this runs to randomize the position in the arena
    {
        //getting dimensions of arena
        var xLow = border[0].transform.position.x + 4;
        var xHigh = border[2].transform.position.x - 4;

        var yLow = border[1].transform.position.y + 4;
        var yHigh = border[3].transform.position.y - 2;

        //returning random values to spawn target in.
        var randPos = new Vector2(Random.Range(xLow, xHigh), Random.Range(yLow, yHigh));
        return randPos;

    }
    void GameRoutine() // runs the routine of the game, spawns holes where they need to be.
    {

        holesLeft = amount; // to track for single player and other factos

        for(int i = 0; i < amount; i++)
        {
            var crateInstance = Instantiate(crate, RandomizePosition(), new Quaternion(), this.gameObject.transform); // spawning crate for sokobot
            var holeInstance = Instantiate(hole, RandomizePosition(), new Quaternion(), this.gameObject.transform); // spanwing hole for sokobot

            holeInstance.GetComponent<SokobotHole>().minigame = this; // grab mother script
            crateInstance.GetComponent<SokobotCrate>().minigame = this; // grab mother script


        }
    }
}
