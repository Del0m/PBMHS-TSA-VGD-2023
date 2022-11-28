//armin delmo, this game is a bunch of cards that get matched with each other. First person to finish wins.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; // for sensing input
using UnityEngine.EventSystems;
using TMPro; // for text mesh modification
public class MemoryMatch : GameHandler
{
    //variables
    public int clicked, cardsLeft; // specifically for tracking cards
    

    private GameObject[] cardsPicked = new GameObject[2]; private GameObject[] cards = new GameObject[10];  // for the purpose of cards

    public override void JumpStart()
    {
        base.JumpStart();
        //build cards
        BuildCards(10); // 10 cards to be built
        CountDown(); // time players for how long they're in minigame
    }
    private void BuildCards(int cardAmount) // grabs given cards, and gives them random numbers
    {
        Debug.Log("building cards");
        //randomizes card data and spawns it on the cards.
        cards = CollectCards();

        Debug.Log(CollectCards());
        cardsLeft = cards.Length;

        var sort = new int[cardAmount];
        //randomizer formula for card picks
        for (int i = 0; i < sort.Length - 1; i++) // to be put in a function L8R
        {
            var randInt = UnityEngine.Random.Range(0, cardAmount); // includes value above since max is exclusive for Random.Range
            for (int j = 0; j < i; j++) // for loop to check for repeat values
            {
                while (randInt == sort[j]) // got same value? reroll
                {
                    randInt = UnityEngine.Random.Range(0, cardAmount);
                }
            }
            sort[i] = randInt;
            for(int k = 0; k < cardAmount; k++)
            {
                Debug.Log(sort[k]);
            }
        }
        var num = 1;
        //putting numbers on the cards
        for(int i = 0; i < cards.Length - 1; i+=2)
        {
            //grabs two from sort; assigns a number to them, rinse and repeat
            //ran twice to pick two cards that will be modified
            Debug.Log(cards[i]);
            cards[sort[i]].GetComponentInChildren<TextMeshProUGUI>().text = num.ToString(); // turns int into string.
            cards[sort[i+1]].GetComponentInChildren<TextMeshProUGUI>().text = num.ToString(); //i+1 to grab the next randomized spot

            //sets all card number colors to be faded so player doesn't know what cards are what
            cards[i].GetComponentInChildren<TextMeshProUGUI>().CrossFadeAlpha(0, 0, false);
            cards[i+1].GetComponentInChildren<TextMeshProUGUI>().CrossFadeAlpha(0, 0, false);


            num++; //make new number to put on card

            
        }


    }
    private void CountDown() // for the purpose of giving players a time limit for the minigame
    {
        var timesUp = Timer(15); // give 15 seconds to player to do minigame

        while(timesUp == false) // hold script till times up
        {
            // do nothing
        }
        Debug.Log("Times Up!");
        gameOrder++;
        //check to see if game order exceeds player count
        if(gameOrder > players.Length)
        {
            gameOrder = 0; // set back to beginning
        }
        //recurse function
        CountDown();
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

    private void AddCards() // when all cards are off the field, add new ones
    {
        if(cardsLeft < 5) // threshold point, usually a pair off.
        {
            //refresh CardGroup
            for(int i = 0; i < cards.Length; i++) // loops through, turns all cards back on
            {
                cards[i].SetActive(true);
            }
            BuildCards(10);
        }


    }
    private GameObject[] CollectCards() // runs FGOWT; grabs all cards on UI field.
    {
        var cardUI = GameObject.FindGameObjectsWithTag("Minigame Card"); // collects cards

        return cardUI;
    }
    string[] cardValue = new string[2] { null, null }; // vars for OnPress
    public void OnPress(GameObject ui) // button flip over 
    {
        Debug.Log(clicked);
        cardsPicked[clicked] = ui;

        cardValue[clicked] = ui.GetComponentInChildren<TextMeshProUGUI>().text; // add value of card onto array to check if cards are the same l8r
        clicked++;
        //ui.GetComponentInChildren<TextMeshProUGUI>().color = new Color(0, 0, 0); // set to black
        ui.GetComponentInChildren<TextMeshProUGUI>().CrossFadeAlpha(255, 0.2f, false);


        

        if (clicked >= 2) // see if two cards are flipped
        {
            //using wait coroutine to show both cards before disappearing
            StartCoroutine(ButtonRoutine(1)); // coroutine wait function and holder of rest of script cuz i don't know how to fix it.... 
        }

        IEnumerator ButtonRoutine(int time) // local IEnumerator since it won't be needed anywhere else.
        {
            yield return new WaitForSeconds(time);

            //two cards are flipped, compare cardValue[] values together, true; score, false; don't score
            if (cardValue[0] == cardValue[1] && cardsPicked[0] != cardsPicked[1])
            {
                //give 1 point
                score[gameOrder]++;
                Debug.Log("You Scored!");

                //disable cards that were picked as right
                cardsPicked[0].SetActive(false);
                cardsPicked[1].SetActive(false);
                //reset clicked and cardsLeft
                cardsLeft -= 2;

                clicked = 0;

                //affix player event system cursor back to an active card
                var activeCard = GameObject.FindGameObjectWithTag("Minigame Card"); // finds an active card
                var eventSystem = GameObject.Find("EventSystem");
                eventSystem.GetComponent<EventSystem>().SetSelectedGameObject(activeCard);
                
                
            }
            else
            {
                Debug.Log("Did NOT score");
                clicked = 0;
            }

            for (int i = 0; i < cards.Length; i++) // for loop to change all cards back to white
            {
                cards[i].GetComponentInChildren<TextMeshProUGUI>().CrossFadeAlpha(0, 0, false);
            }
            AddCards(); // to be ran to see if cards need to be restocked.
        }
    }


}
