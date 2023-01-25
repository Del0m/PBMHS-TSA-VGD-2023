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
    public GameObject[] colorFlag; // color flag to show
    public GameObject[] colorPlatform; // colored platforms that'll be dropped

    [Header("Game Settings")]
    public float dropTime; // time till they drop
    public int dropAmount; // amount the platforms will drop


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

    IEnumerator DropColors() // drop colors that ain't the chosen one
    {
        for(int i = 0; i < dropAmount; i++)
        {
            var colorChosen = ChooseColor(); // color they will have to go onto
            // show color to player
            Debug.Log(colorString[colorChosen]); // show in testing 

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
            yield return new WaitForSeconds(2); // wait for them do DIEE >:}
            for(int k = 0; k < colorString.Length; k++)
            {
                var colorScript = colorPlatform[k].GetComponent<ColorPlatform>();
                colorScript.dropPlatform = false;
            }
            dropTime -= (dropTime / 10); // reduce drop time to up the stakes
        }
    }

}
