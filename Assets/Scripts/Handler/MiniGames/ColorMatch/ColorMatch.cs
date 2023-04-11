using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    public TextMeshProUGUI dropleft; // drops left to show to player

    private int previousColor; // previous color used in minigame randomization

    [Header("Game Settings")]

    public float dropTime; // time till they drop
    public int dropAmount; // amount the platforms will drop

    public float originalSpeed;
    public float originalJumpPower;

    [HideInInspector]
    public bool hasStopped = false;
    void Start()
    {

        uiManager = GameObject.FindGameObjectWithTag("PlayerUIManager").GetComponent<PlayerUIManager>();
        
        StartCoroutine(StartGame()); // starting game and bringing players into the game
        ModifyPlayerStats(true); // decrease player speed

        StartCoroutine(DropColors()); 
    }
    int ChooseColor()
    {
        var randomColor = Random.Range(0, colorString.Length);
        while(randomColor == previousColor) // prevent same color from showing up
        {
            Debug.Log("Choosing new color, accidentally repeated.");
            if(randomColor != previousColor) // break statement to prevent infinite running of outer while loop
            {
                break;
            }
            randomColor = Random.Range(0, colorString.Length);
        }
        // setting previous color to prevent same color from coming up again
        previousColor = randomColor;
        // randomly select color
        return randomColor;

    }
    void ModifyPlayerStats(bool decrease)
    {
        var plr = GameObject.FindGameObjectsWithTag("Player");
        // coroutine to lower players speed
        if(decrease)
        {
            for (int i = 0; i < plr.Length; i++)
            {
                // grab stats from players
                Debug.Log(i + " Player's stats are being changed!");


                var playerStat = plr[i].GetComponent<PlayerStats>();

                playerStat.speed /=  2;
            }
        }
        else
        {
            for(int i = 0; i < plr.Length; i++)
            {
                Debug.Log("Restting stats!");
                var playerStat = plr[i].GetComponent<PlayerStats>();

                playerStat.speed = originalSpeed;
                playerStat.jumpPower = originalJumpPower;
            }
        }
    }

    IEnumerator DropColors() // drop colors that ain't the chosen one
    {                            
        yield return new WaitForSeconds(5);

        var platScript = flagPlatform.GetComponent<MovingPlatform>(); // platform script to move platform above color



        for (int i = 0; i < dropAmount; i++)
        {
            dropleft.text = "Drops Left: " + (dropAmount - i).ToString();
            var colorChosen = ChooseColor(); // color they will have to go onto
            // show color to player
            Debug.Log(colorString[colorChosen]); // show in testing 

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
        ModifyPlayerStats(false); // increase player speed oncemore!
        StartCoroutine(EndGame(winner)); // fix at a later date, doesn't give all players a win
    }
    public IEnumerator LoseGame() // when all players have lost
    {
        hasStopped = true;
        uiManager.ChangeUI(true, uiManager.loseUI);
        ModifyPlayerStats(false); // bring players stats back to normal


        yield return new WaitForSeconds(5);
        for(int i = 0; i < player.Count; i++)
        {
            player[i].GetComponent<PlayerStats>().wins -= 1;
        }

        StartCoroutine(EndGame(CheckWinner())); // game over for single player


    }
}
