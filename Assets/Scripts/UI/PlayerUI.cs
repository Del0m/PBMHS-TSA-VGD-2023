//armin delmo; 1/15/22; purpose of this program is to store all player UI elements to be edited by the game
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public int playerID; // tells them what player they control
    private PlayerUIManager uiManager; // to find the players
    // buff objects / number object
    public GameObject[] buffUI;
    public TextMeshProUGUI wins;
    public GameObject currentTurn;

    public void Start()
    {
        uiManager = GameObject.FindGameObjectWithTag("PlayerUIManager").GetComponent<PlayerUIManager>();
    }

    public void CheckBuff() // check the buffs of the player they are representing, show it via UI
    {
        var buff = GameObject.FindGameObjectWithTag("Player Manager").GetComponent<PlayerManager>().player[playerID].GetComponent<PlayerStats>().buff;

        for(int i = 0; i < buff.Count; i++)
        {
            try
            {
                if (buff[i])
                {
                    buffUI[i].GetComponent<Image>().color = buff[i].color;
                }
                else
                {
                    buffUI[i].GetComponent<Image>().color = new Color(0, 0, 0, 0);
                }
            }
            catch (System.Exception)
            {

                throw;
            }

        }
    }
    public void UpdateWins(int win) // updates the win counter on the player
    {
        wins.text = win.ToString();
    }
}
