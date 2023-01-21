using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageScript : MonoBehaviour
{
    public Sprite[] knightIcon; // playable knight character in game; detail
    public Sprite[] wizardIcon; // playable wizard character in game; detail
    public Sprite[] ImageSelection() // returns the texture2d array that is selected via switch
    {
        var randomNum = Random.Range(0, 2); // 0 - 1 atm b/c of one image only atm

        switch (randomNum) // switch with all random imagery.
        {
            case 0:
                return knightIcon;
            case 1:
                return wizardIcon;

            default:
                return null;
        }
    }
}
