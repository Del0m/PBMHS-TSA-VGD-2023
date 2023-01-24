using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageScript : MonoBehaviour
{
    public Sprite[] knightIcon; // playable knight character in game; detail
    public Sprite[] wizardIcon; // playable wizard character in game; detail

    public int[] pictureChosen; // to prevent multiple runs
    public Sprite[] ImageSelection() // returns the texture2d array that is selected via switch
    {

        switch (ChangeRepeat()) // switch with all random imagery.
        {
            case 0:
                return knightIcon;
            case 1:
                return wizardIcon;

            default:
                return null;
        }
    }
    public int ChangeRepeat() // change repeat drawings of the images
    {
        var randNum = Random.Range(0, 2); // 0 - 1 atm b/c of one image only atm

        for (int i = 0; i < pictureChosen.Length; i++) // for loop to change repeats
        {
            if(randNum == pictureChosen[i]) { randNum = Random.Range(0, 2); }
        }
        for (int i = 0; i < pictureChosen.Length; i++)
        {
            if (randNum == pictureChosen[i]) { ChangeRepeat(); break; } // recurse function until no repeat
        }

        return randNum;
    }
}
