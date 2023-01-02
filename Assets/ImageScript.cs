using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageScript : MonoBehaviour
{
    public Sprite[] coconutSplice; // coconut.jpg
    public Sprite[] ImageSelection() // returns the texture2d array that is selected via switch
    {
        var randomNum = Random.Range(0, 1); // 0 - 1 atm b/c of one image only atm

        switch (randomNum) // switch with all random imagery.
        {
            case 0: // coconut.jpg
                return coconutSplice;
            default:
                return null;
        }
    }
}
