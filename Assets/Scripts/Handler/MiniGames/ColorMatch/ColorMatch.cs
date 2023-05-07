using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Tilemaps;
using UnityEngine;

public class ColorMatch : GameHandler
{
    [Header("Colors")] // order in arrays is: blue, red, green, yellow
    public string[] colorString =
    {
        "blue",
        "red",
        "green",
        "yellow"
    };
    public Color[] flagColor; // color flag to show
    public SpriteRenderer flagObject;
    public GameObject[] colorPlatform; // colored platforms that'll be dropped
    public GameObject flagPlatform;
    [Header("UI")]

    private int previousColor; // previous color used in minigame randomization

    [Header("Game Settings")]

    public float dropTime; // time till they drop
    public int dropAmount; // amount the platforms will drop

    [HideInInspector]
    public bool hasStopped = false;
    public float speedModifier; // usually two
    void Start()
    {

        uiManager = GameObject.FindGameObjectWithTag("PlayerUIManager").GetComponent<PlayerUIManager>();
        
        StartCoroutine(StartGame()); // starting game and bringing players into the game
        ModifyPlayerStats((speedModifier / 4), (speedModifier / 2)); // decrease player speed

        StartCoroutine(DropColors()); 
    }
    public override void IncreaseDifficulty() // decrease time to drop and increase amount of drops
    {
        base.IncreaseDifficulty();
        dropAmount = ((int)(dropAmount * multiplier));
        dropTime = ((int)(dropTime / multiplier));

        minimumToWin = 1;
    }
    public override IEnumerator StartGame()
    {
        // make the difficulty add more drops and less time
        StartCoroutine(base.StartGame());
        yield return new WaitForEndOfFrame(); // wait for StartGame() to finish loading

        // allow players to move
        for (int i = 0; i < player.Count; i++)
        {
            var playerMovement = player[i].GetComponent<PlayerMovement>();

            playerMovement.GameSwitch(true);
        }
        yield return null;
    }
    int ChooseColor()
    {
        var randomColor = Random.Range(0, colorString.Length);
        if(randomColor == previousColor) // recursion to prevent same color
        {
            return ChooseColor();
        }

        // setting previous color to prevent same color from coming up again
        previousColor = randomColor;
        // randomly select color
        return randomColor;
    }
    void ModifyPlayerStats(float walkMulti, float jumpMulti)
    {
        var plr = GameObject.FindGameObjectsWithTag("Player");
        // coroutine to lower players speed
        for (int i = 0; i < plr.Length; i++)
        {
                // grab stats from players

                var playerStat = plr[i].GetComponent<PlayerStats>();
                playerStat.speed *= walkMulti;
                playerStat.jumpPower *= jumpMulti;
        }
    }

    IEnumerator DropColors() // drop colors that ain't the chosen one
    {                            
        yield return new WaitForSeconds(5);

        var platScript = flagPlatform.GetComponent<MovingPlatform>(); // platform script to move platform above color



        for (int i = 0; i < dropAmount; i++)
        {
            // notifying how many drops the players have left
            gameUI.ModifyText("" + (dropAmount - i));

            var colorChosen = ChooseColor(); // color they will have to go onto
            // show color to player

            // show color to players
            flagObject.color = flagColor[colorChosen];

            // move flag platform above the platform to go to
            var xPos = colorPlatform[colorChosen].GetComponent<ColorPlatform>().dropPos;
            platScript.point[0] = platScript.pointStorage[colorChosen];

            yield return new WaitForSeconds(dropTime); // time they have left till drop
            
            // for loop to drop all those platforms that ain't it
            for(int j = 0; j < colorString.Length; j++)
            {
                if(colorChosen != j)
                {
                    var colorScript = colorPlatform[j].GetComponent<ColorPlatform>();
                    colorScript.dropPlatform = true;
                }
            }
            yield return new WaitForSeconds(5); // wait for them do DIEE >:}
            // set flag to white
            flagObject.color = Color.white;
            for(int k = 0; k < colorString.Length; k++)
            {
                var colorScript = colorPlatform[k].GetComponent<ColorPlatform>();
                colorScript.dropPlatform = false;
            }
            dropTime -= (dropTime / 8); // reduce drop time to up the stakes
            yield return new WaitForSeconds(5);
        }
        ModifyPlayerStats(speedModifier*4, speedModifier * 1.25f); // increase player speed oncemore!
        StartCoroutine(EndGame(winner)); // fix at a later date, doesn't give all players a win
    }
    public IEnumerator LoseGame() // when all players have lost
    {
        hasStopped = true;
        uiManager.ChangeUI(true, uiManager.loseUI);
        ModifyPlayerStats(speedModifier*4, speedModifier*2f); // bring players stats back to normal


        yield return new WaitForSeconds(5);
        for(int i = 0; i < player.Count; i++)
        {
            player[i].GetComponent<PlayerStats>().wins -= 1;
        }

        StartCoroutine(EndGame(CheckWinner())); // game over for single player


    }
}
