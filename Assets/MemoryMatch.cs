//armin delmo, this game is a bunch of cards that get matched with each other. First person to finish wins.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; // for sensing input
using TMPro; // for text mesh modification
public class MemoryMatch : GameHandler
{
    //variables
    public int clicked;
    private int[] score = new int[4] { 0, 0, 0, 0 };

    GameObject[] cardsPicked = new GameObject[2]; // for the purpose of cards
    public override void JumpStart()
    {
        base.JumpStart();
        //build cards
        BuildCards();
    }
    private void BuildCards()
    {
        Debug.Log("building cards");
        //randomizes card data and spawns it on the cards.
        var cards = CollectCards();

        var sort = new int[10];
        //randomizer formula for card picks
        for (int i = 0; i < sort.Length - 1; i++) // to be put in a function L8R
        {
            var randInt = Random.Range(0, 10); // includes value above since max is exclusive for Random.Range
            Debug.Log("Sorting!");
            for (int j = 0; j < i; j++) // for loop to check for repeat values
            {
                while (randInt == sort[j]) // got same value? reroll
                {
                    randInt = Random.Range(0, 10);
                }
            }
            sort[i] = randInt;

        }
        var num = 1;
        //putting numbers on the cards
        for(int i = 0; i < cards.Length - 1; i+=2)
        {

            //grabs two from sort; assigns a number to them, rinse and repeat
            Debug.Log("Assigning value " + num + " to cards:" + i + " and " + (i + 1));
            //ran twice to pick two cards that will be modified
            cards[sort[i]].GetComponentInChildren<TextMeshProUGUI>().text = num.ToString(); // turns int into string.
            cards[sort[i+1]].GetComponentInChildren<TextMeshProUGUI>().text = num.ToString(); //i+1 to grab the next randomized spot

            num++; //make new number to put on card
        }

    }
    private void PlayerAction() // will turn on and off player ability to manipulate UI for the game.
    {
        //for loop to enable correct action map
        for(int i = 0; i < players.Length; i++)
        {
            players[i].GetComponent<PlayerInput>().SwitchCurrentActionMap("UI");
        }
        //for loop to disable all other player input
        for(int i = 0; i < players.Length; i++)
        {
            players[i].GetComponent<PlayerInput>().DeactivateInput();
        }
        players[gameOrder - 1].GetComponent<PlayerInput>().ActivateInput();
    }
    private void CheckCards() // when player flips any two given cards, see if they have the same number, then give them a score.
    {

    }
    private GameObject[] CollectCards() // runs FGOWT; grabs all cards on UI field.
    {
        var cardUI = GameObject.FindGameObjectsWithTag("Minigame Card"); // collects cards

        return cardUI;
    }
    string[] cardValue = new string[2] { null, null };
    public void OnPress(GameObject ui) // button flip over 
    {
        Debug.Log("Pressing " + ui);
        Debug.Log(clicked);
        cardsPicked[clicked] = ui;

        cardValue[clicked] = ui.GetComponentInChildren<TextMeshProUGUI>().text; // add value of card onto array to check if cards are the same l8r
        clicked++;
        ui.GetComponentInChildren<TextMeshProUGUI>().color = new Color(0, 0, 0); // set to black

        if (clicked >= 2) // see if two cards are flipped
        {
            //two cards are flipped, compare cardValue[] values together, true; score, false; don't score
            if(cardValue[0] == cardValue[1] && cardsPicked[0] != cardsPicked[1])
            {
                //give 1 point
                score[gameOrder]++;
                Debug.Log("You Scored!");
                //reset clicked
                clicked = 0;
            }
            else
            {
                Debug.Log("Did NOT score");
                clicked = 0;
            }
            //set all text colors to white (make invisible again)
            var cards = CollectCards();

            for (int i = 0; i < cards.Length; i++) // for loop to change all cards
            {
                cards[i].GetComponentInChildren<TextMeshProUGUI>().color = new Color(255, 255, 255);
            }
        }

    }
}
