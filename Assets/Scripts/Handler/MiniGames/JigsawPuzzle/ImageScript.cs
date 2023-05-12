using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class ImageScript : MonoBehaviour
{
    public Sprite[] knightIcon; // playable knight character in game; detail
    public Sprite[] wizardIcon; // playable wizard character in game; detail
    public Sprite[] witchIcon; // witch from pinata game
    public Sprite[] rogueIcon;

    static int[] pictureChosen = new int[4] {99,99,99,99}; // to prevent multiple runs
    static int increment = 0; // increments every time a player chooses a photo
    public Sprite[] ImageSelection() // returns the texture2d array that is selected via switch
    {

        switch (ChangeRepeat()) // switch with all random imagery.
        {
            case 0:
                return knightIcon;
            case 1:
                return wizardIcon;
            case 2:
                return witchIcon;
            case 3:
                return rogueIcon;

            default:
                return null;
        }
    }
    public int ChangeRepeat() // change repeat drawings of the images
    {
        var randNum = Random.Range(0, 4); // max two pictures
        var repeat = true;
        while(repeat)
        {
            for(int i = 0; i < pictureChosen.Length;)
            {
                if(randNum == pictureChosen[i])
                {
                    randNum = Random.Range(0, 2); // change number
                    i = 0;
                }
                else
                {
                    i++;
                }
            }
            repeat = false;
        }
        pictureChosen[increment] = randNum;
        increment++;
        return randNum;
    }
}
