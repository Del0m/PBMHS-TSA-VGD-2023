//armin delmo; Mathdash.cs; this is the math dash minigame
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MathDash : GameHandler
{
    //problem variables
    public int[] number = new int[2]; // numbers that will be used for the problems.
    public char problemType;
    public int answer;
    public GameObject cardPrefab;

    public GameObject[] card;

    //ui
    public TextMeshProUGUI text; // prompt for players to solve

    [Header("Math Dash Variables")]
    public int time;

    // Start is called before the first frame update
    void Start()
    {
        //uiManager = GameObject.FindGameObjectWithTag("PlayerUIManager").GetComponent<PlayerUIManager>();

        StartCoroutine(StartGame()); // teleport players to the minigame.
        StartCoroutine(NewProblem()); // makes new problem to have player solve.
        
    }
    public override void IncreaseDifficulty() // set time limit 
    {
        base.IncreaseDifficulty();
        minimumToWin = ((int)(5 * multiplier)); // minimum cards to get right in order to pass
        time = ((int)(time / multiplier));
    }
    public override IEnumerator PreGameRoutine() // adding a timer to the minigame in singleplayer
    {
        IncreaseDifficulty();
        yield return StartCoroutine(base.PreGameRoutine());

        yield return new WaitForSeconds(3); // countdown
        yield return StartCoroutine(gameUI.Timer(time)); // running the timer
    }
    public override IEnumerator StartGame()
    {
        StartCoroutine(base.StartGame());

        yield return new WaitForEndOfFrame(); // wait for StartGame() to finish loading

        // allow players to move in the game
        for (int i = 0; i < player.Count; i++)
        {
            var playerMovement = player[i].GetComponent<PlayerMovement>();

            playerMovement.GameSwitch(true);
        }
        yield return null;
    }
    void MakeProblem() // making the problem that the players have to solve
    {
        var randomProblem = Random.Range(0, 4);

        switch(randomProblem) //switch statement to find if it will be +, -, *, or /
        {
            case 0: // add
                problemType = '+';

                //making nums for problem
                number[0] = Random.Range(0, 300);
                number[1] = Random.Range(0, 300);

                //defining answer
                answer = number[0] + number[1];
                break;
            case 1: // minus
                problemType = '-';

                //making nums for problem
                number[0] = Random.Range(0, 300);
                number[1] = Random.Range(0, 300);

                //defining answer
                answer = number[0] - number[1];
                break;
            case 2: // mult
                problemType = '*';

                //making nums for problem
                number[0] = Random.Range(1, 15);
                number[1] = Random.Range(1, 15);

                //defining answer
                answer = number[0] * number[1];
                break;
            case 3: // div
                problemType = '/';

                //making nums for problem; using one as minimum to prevent div error
                number[0] = Random.Range(1, 30);
                number[1] = Random.Range(1, 30);

                //running loop to ensure whole number is gotten for players
                while (number[0] % number[1] != 0)
                {
                    number[0] = Random.Range(1, 30);
                    number[1] = Random.Range(1, 30);
                }
                //defining answer
                answer = number[0] / number[1];
                break;
        }
    }
    IEnumerator SpawnCards(int answer) // problem cards the players can grab to solve the problem
    {
        //randomly select amount of cards to have on board.
        var cardAmount = Random.Range(5, 11); ; // 5-10 cards will be spawned

        var cardDisplay = new int();
        yield return new WaitForSeconds(0.5f);

        var rightCard = Instantiate(cardPrefab, RandPosition(), new Quaternion(), this.gameObject.transform); // spawning new card.
        rightCard.GetComponentInChildren<Card>().value = answer;
        rightCard.SetActive(true); // allow card to be seen in game

        for (int i = 0; i < cardAmount; i++) // for loop to spawn in cards with random values
        {
            var randomized = new int[] { Random.Range(1, 11), Random.Range(1, 6) };
            var rand = Random.Range(0, 5);
            switch (rand) // switch statement to decide what is on the card.
            {
                case 0:
                    cardDisplay = answer + randomized[0];
                    //instantiate card with +1-10
                    break;
                case 1:
                    cardDisplay = -answer;
                    //instantiate card with answer negative
                    break;
                case 2:
                    cardDisplay = answer - randomized[0];
                    //instantiate card with answer - 10
                    break;
                case 3:
                    cardDisplay = answer - randomized[1];
                    //instantiate card with answer +- (1,5)
                    break;
                case 4:
                    cardDisplay = answer + randomized[0] * 2;
                    //instantiate card with answer
                    break;
            }
            //spawn in object in random area in minigame arena.
            var newCard = Instantiate(cardPrefab, RandPosition(), new Quaternion(), this.gameObject.transform); // spawning new card.
            newCard.SetActive(true); // allow to be seen in the scene
            newCard.GetComponentInChildren<Card>().value = cardDisplay; // setting what is on the card
            newCard.tag = "Minigame Element"; // the card prefab won't have the tag to not be deleted.
        }
        //add all cards to array
        card = GameObject.FindGameObjectsWithTag("Minigame Element");

        //update ui
        text.text = "What is: " + number[0] + " " + problemType + " " + number[1] + "?";
    }
    public bool CheckAnswer(GameObject plr, int guess) // award player with points if correctly slammed right card
    {
        
        if(guess == answer)
        {
            text.text = "Correct! The Answer is: " + answer; // changes text to show they got it correct.
            gameScore[plr.GetComponent<PlayerStats>().turnOrder]++; // player position in score array is awarded a point
            this.StartCoroutine(NewProblem()); // making new problem for player

            // change cards gotten correct
            var totalScore = gameScore[0] + gameScore[1] + gameScore[2] + gameScore[3];
            gameUI.ModifyText("" + totalScore);
            return true;
        }
        else
        {
            return false;
            //update UI on player.
        }
    }
    IEnumerator NewProblem() // procedure to put new problem on the board.
    {
        //delete all current cards
        for (int i = 0; i < card.Length; i++)
        {
            Destroy(card[i].gameObject); // destroy card in array
        }
        yield return new WaitForSeconds(1);
        MakeProblem(); // make factors and answer
        yield return new WaitForSeconds(2); // wait for two seconds so everything initalizes correctly.
        StartCoroutine(SpawnCards(answer)); // spawn cards in map
    }
}
