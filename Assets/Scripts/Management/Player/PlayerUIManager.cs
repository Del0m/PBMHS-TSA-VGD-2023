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
    public TextMeshProUGUI countdownUI; // to be counting down before every minigame to show the player when it starts!

    [Header("DiceUI Elements")]
    public GameObject diceSprite;
    public Sprite[] diceImage;

    [Header("Game Specific UI")]
    public GameObject healthBarUI; // health bar for pinata minigame
    public GameObject loseUI; // to be enabled when all players lose the game

    public TextMeshProUGUI timeLeftUI; // to tick down
    public bool timesUp;

    [Header("Single Player UI")]
    public GameObject gameOverUI;
    public TextMeshProUGUI level; // updated at the end of every game

    // this will be ran in the PlayerManager
    private void Start()
    {
        this.gameObject.tag = "PlayerUIManager";
    }
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
    public void UpdateLevel(int lvl)
    {
        if(level != null)
        {
            level.text = lvl.ToString(); // sets level in function
        }
        else
        {
            Debug.LogError("Can't find Single Player UI Level text!");
            return;
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

    private void ResetUI() // reset ui to default values
    {
        timeLeftUI.text = "Time Left: ";

        var healthBarRect = healthBarUI.GetComponent<RectTransform>();

        healthBarRect.sizeDelta = new Vector2(100, 10);
    }
    public void ChangeUI(bool minigame) // changes the UI from minigame to board, vice versa
    {
        boardUI.SetActive(!minigame);
        minigameUI.SetActive(minigame);
        ResetUI();
    }
    public void ChangeUI(bool minigame, GameObject obj) // same as ChangeUI, just overload to add extraUI if needed
    {
        ChangeUI(minigame); // default settings from previous
        obj.SetActive(minigame); // setting object active

        Debug.Log("object being changed is:" + obj);

        // if statement to check if it is a clock

    }
    public IEnumerator UpdateClock(int time) // runs the countdown in addition to the time that will be left in the minigame
    {
        ChangeUI(true, timeLeftUI.gameObject); // enable ui

        timesUp = false;
        for(int i = 0; i < time; i++)
        {
            yield return new WaitForSeconds(1f);
            timeLeftUI.text = "Time Left: " + (time - i); // do math to show time left.
        }
        timesUp = true;
        yield return new WaitForSeconds(1);
        timesUp = false;

        ChangeUI(false, timeLeftUI.gameObject); // disable ui
        timeLeftUI.text = "Time Left: ";
    }
    public IEnumerator CountDown(int time, TextMeshProUGUI obj)
    {
        obj.gameObject.SetActive(true);

        for(int i = 0; i < time; i++)
        {
            obj.text = (time - i).ToString(); // show time left
            yield return new WaitForSeconds(1f);
        }

        obj.text = "Go!";
        yield return new WaitForSeconds(1f);

        obj.gameObject.SetActive(false);
    }
}
