//armin delmo; 1/15/22; purpose of this program is to store all player UI elements to be edited by the game
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    // buff objects / number object
    public GameObject buffA;
    public GameObject buffB;
    public TextMeshProUGUI wins;
    public GameObject currentTurn;

    // booleans to detect which buff icons are enabled
    bool[] buffIconUsed;

    public void AddBuff(string buff, Color buffColor) // show the buff to the player
    {
        if (buffIconUsed[0] == true) // check if first icon being used
        {
            buffB.GetComponent<Image>().color = buffColor; // change to according color
            buffIconUsed[1] = true;
        }
        else if (buffIconUsed[0] == false)
        {
            buffA.GetComponent<Image>().color = buffColor;
            buffIconUsed[0] = true;
        }
        else
        {
            Debug.LogError("Both buff slots are currently taken.");
        }
    }
    public void UpdateWins(int win) // updates the win counter on the player
    {
        wins.text = win.ToString();
    }
}
