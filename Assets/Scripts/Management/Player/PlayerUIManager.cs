using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour
{
    public GameObject[] playerUI; // the UI that shows the player's wins, icon and buffs
    public GameObject roundCounter; // the UI that shows the current round the game is on

    [Header("UI Types")] // to be en/disabled through ChangeUI() and overloads
    public GameObject boardUI;
    public GameObject minigameUI;

    [Header("DiceUI Elements")]
    public GameObject diceSprite;
    public Sprite[] diceImage;

    public GameObject healthBarUI; // health bar for pinata minigame

    // this will be ran in the PlayerManager
    public void InitalizeUI() // this is to hide UI that won't be used in the game due to a lack of plrs
    {
        var playerCount = GameObject.FindGameObjectsWithTag("Player").Length; // grabbing the amount of players in the game

        //hide all ui for loop
        for(int i = 0; i < playerUI.Length; i++)
        {
            playerUI[i].SetActive(false);
        }

        // unhide used UI for loop
        for(int i = 0; i < playerCount; i++)
        {
            playerUI[i].SetActive(true);
        }
    }
    public void UpdateRound(int round)
    {
        roundCounter.GetComponent<TextMeshProUGUI>().text = round + " / 10";
    }
    public void UpdateDiceUI(int num) // updates UI for the dice for the board
    {
        diceSprite.GetComponent<Image>().sprite = diceImage[num];
        if(num == 0)
        {
            diceSprite.GetComponent<Animator>().playbackTime = 0; // reset dice back to middle
        }
    }
    public void ChangeUI(bool minigame) // changes the UI from minigame to board, vice versa
    {
        boardUI.SetActive(!minigame);
        minigameUI.SetActive(minigame);

    }
    public void ChangeUI(bool minigame, bool healthBar) // same as ChangeUI, just overload to add health bar if needed
    {
        ChangeUI(minigame); // default settings from previous
        healthBarUI.SetActive(healthBar); // setting healthbar
    }
}
