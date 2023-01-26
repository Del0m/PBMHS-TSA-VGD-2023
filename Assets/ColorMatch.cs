using System.Collections;
using System.Collections.Generic;
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

    [Header("Game Settings")]
    public float dropTime; // time till they drop
    public int dropAmount; // amount the platforms will drop

    private float originalSpeed;
    private float originalJumpPower;
    void Start()
    {
        StartCoroutine(StartGame(true)); // starting game and bringing players into the game
        StartCoroutine(DropColors()); 
    }
    int ChooseColor()
    {
        // randomly select color
        return Random.Range(0, colorString.Length);

    }
    void ModifyPlayerStats(bool decrease)
    {
        // grab stats from players

        var statGrab = player[0].GetComponent<PlayerStats>();

        originalSpeed = statGrab.speed;
        originalJumpPower = statGrab.jumpPower;

        // coroutine to lower players speed
        if(decrease)
        {
            for(int i = 0; i < player.Length; i++)
            {
                var playerStat = player[i].GetComponent<PlayerStats>();

                playerStat.speed /=  2;
                playerStat.jumpPower /= 1.5f;
            }
        }
        else
        {
            for(int i = 0; i < player.Length; i++)
            {
                var playerStat = player[i].GetComponent<PlayerStats>();

                playerStat.speed = originalSpeed;
                playerStat.jumpPower = originalJumpPower;
            }
        }
    }

    IEnumerator DropColors() // drop colors that ain't the chosen one
    {
        yield return new WaitForSeconds(5);
        ModifyPlayerStats(true); // decrease player speed

        var platScript = flagPlatform.GetComponent<MovingPlatform>(); // platform script to move platform above color



        for (int i = 0; i < dropAmount; i++)
        {
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
            dropTime -= (dropTime / 10); // reduce drop time to up the stakes
            yield return new WaitForSeconds(5);
        }
        ModifyPlayerStats(false); // increase player speed oncemore!
    }
    public IEnumerator LoseGame() // when all players have lost
    {
        uiManager.ChangeUI(true, uiManager.loseUI);
        yield return new WaitForSeconds(5);
        for(int i = 0; i < player.Length; i++)
        {
            player[i].GetComponent<PlayerStats>().wins -= 1;
        }
    }
}
