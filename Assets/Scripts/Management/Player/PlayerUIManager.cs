using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour
{
    [Header("Managers")]
    public PlayerManager manager; // to grab the player.length and other variables
    
    public GameObject[] playerUI; // the UI that shows the player's wins, icon and buffs
    public GameObject roundCounter; // the UI that shows the current round the game is on

    [Header("UI Types")] // to be en/disabled through ChangeUI() and overloads
    public GameObject boardUI;
    public GameObject minigameUI;
    public TextMeshProUGUI countdownUI; // to be counting down before every minigame to show the player when it starts!
    public GameObject successUI; // ui to be played at the end of a game

    [Header("DiceUI Elements")]
    public GameObject diceSprite;
    public Sprite[] diceImage;

    [Header("Game Specific UI")]
    public GameObject healthBarUI; // health bar for pinata minigame
    public GameObject healthBar; // bar itself
    public GameObject loseUI; // to be enabled when all players lose the game
    public GameObject scoreLeftUI; // single player mathdash to show end-user how many more they need right.

    [Header("Single Player UI")]
    public GameObject gameOverUI;
    public TextMeshProUGUI level; // updated at the end of every game
    public bool isSinglePlayer;

    // this will be ran in the PlayerManager
    private void Start()
    {
        this.gameObject.tag = "PlayerUIManager";
        manager = GameObject.FindGameObjectWithTag("Player Manager").GetComponent<PlayerManager>();
    }

    public void InitalizeUI() // this is to hide UI that won't be used in the game due to a lack of plrs
    {
        var playerCount = manager.player.Count; // grabbing the amount of players in the game

        // unhide used UI for loop
        for(int i = 0; i < playerCount; i++)
        {
            playerUI[i].SetActive(true);
        }
        ValueUpdate(); // show player their turn
    }
    public void ValueUpdate() // update all the values on the UI
    {
        var playerObject = manager.player;
        for(int i = 0; i < playerObject.Count; i++)
        {
            var ui = playerUI[i].GetComponent<PlayerUI>();

            if (playerObject[i].GetComponent<PlayerStats>().turnOrder == manager.turn.currentTurn)
            {
                ui.currentTurn.SetActive(true);
            }
            else
            {
                ui.currentTurn.SetActive(false);
            }
        }
        if(!manager.singlePlayer)
        {
            UpdateRound(manager.turn.roundsElapsed);
        }
        UpdatePlayerUI(); // update the buffs and wins on each player
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
        roundCounter.GetComponent<TextMeshProUGUI>().text = round + " / " + "6";
    }
    public void UpdateDiceUI(int num) // updates UI for the dice for the board
    {
        diceSprite.GetComponent<Image>().sprite = diceImage[num];
        if(num == 0)
        {
            diceSprite.GetComponent<Animation>().Rewind(); // reset dice back to middle
        }
    }
    public void UpdatePlayerUI() // update the player's ui to show buffs
    {

        for(int i = 0; i < manager.player.Count; i++)
        {
            var ui = playerUI[i].GetComponent<PlayerUI>();

            ui.wins.GetComponent<TextMeshProUGUI>().text = manager.player[i].GetComponent<PlayerStats>().wins.ToString();
            ui.CheckBuff();
        }
    }

    private void ResetUI() // reset ui to default values
    {
        /*
        var healthBarRect = healthBar.GetComponent<RectTransform>();

        healthBarRect.sizeDelta = new Vector2(100, 10);
        */
        if(!manager.singlePlayer)
        {
            UpdateRound(manager.turn.roundsElapsed);
            ValueUpdate();
        }
    }
    public void ChangeUI(bool minigame) // changes the UI from minigame to board, vice versa
    {
        boardUI.SetActive(!minigame);
        //minigameUI.SetActive(minigame);
        ResetUI();
    }
    public void ChangeUI(bool minigame, GameObject obj) // same as ChangeUI, just overload to add extraUI if needed
    {
        ChangeUI(minigame); // default settings from previous
        obj.SetActive(minigame); // setting object active

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
    public void UIPopUpWrapper(GameObject target)
    {
        StartCoroutine(UIPopUp(target));
    }
    public void UIPopUpWrapper(GameObject target, int winner)
    {
        StartCoroutine(UIPopUp(target, winner));
    }
    bool isntActing; // to put down the UI
    public IEnumerator UIPopUp(GameObject target) // pops up UI to show player
    {
        isntActing = true; // to put down the UI

        target.SetActive(true);
        for(int i = 0; i < manager.player.Count; i++)
        {
            var rb = manager.player[i].GetComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Static;
        }
        yield return new WaitForSeconds(1.5f); // wait for player to read
        while(isntActing)
        {
            if(!target.activeInHierarchy)
            {
                target.SetActive(true);
            }

            if (manager.player[0].GetComponent<PlayerMovement>().acting == true)
            {
                yield return new WaitForSeconds(1f);
                isntActing = false;
                target.SetActive(false);

                //successUI.GetComponent<UIFade>().text[0].text = "Everyone lost!"; // resetting success ui

            }
            yield return new WaitForEndOfFrame();
        }
        if(isSinglePlayer)
        {
            for (int i = 0; i < manager.player.Count; i++)
            {
                var rb = manager.player[i].GetComponent<Rigidbody2D>();
                rb.bodyType = RigidbodyType2D.Dynamic;
            }
        }

    }
    public IEnumerator UIPopUp(GameObject target, int winner)
    {
        if(!isSinglePlayer)
        {
            successUI.GetComponent<UIFade>().text[0].text = "Player " + winner + " has won!";
        }
        if(winner < 1) // variable that is sent when no player has won
        {
            successUI.GetComponent<UIFade>().text[0].text = "Nobody has won.";

        }
        StartCoroutine(UIPopUp(target));
        yield break;
    }
    public IEnumerator ShowUI(GameObject ui) // shows ui to the player, particularly for minigame instruction.
    {
        // this will slow down the game until the player turns on a button
        ui.SetActive(true);
        // slow down the game
        Time.timeScale = 0.0001f;

        var cont = false; // set the player to continue to game to false

        while(!cont)
        {
            // check if player is acting
            if (manager.player[0].GetComponent<PlayerMovement>().acting)
            {
                cont = true; // break the loop
                Time.timeScale = 1; // reset time to norm
                break;
            }
            yield return new WaitForEndOfFrame(); // prevent overflow
        }
        ui.SetActive(false); // remove the ui from the player's screen
        yield return null; // end


    }
}
