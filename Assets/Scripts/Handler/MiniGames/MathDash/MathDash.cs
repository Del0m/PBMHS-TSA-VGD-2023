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

    private Vector2 randPos;



    //ui
    public TextMeshProUGUI text; // prompt for players to solve
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(TeleportPlayers(false,false, true)); // teleport players to the minigame.
        StartCoroutine(NewProblem()); // makes new problem to have player solve.
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
                print(answer);
                break;
        }
    }
    void RandomizePosition() // this runs to randomize the position in the arena
    {
        //getting dimensions of arena
        var xLow = border[0].transform.position.x+4;
        var xHigh = border[2].transform.position.x-4;

        var yLow = border[1].transform.position.y+4;
        var yHigh = border[3].transform.position.y-2;

        //returning random values to spawn target in.
        randPos = new Vector2(Random.Range(xLow,xHigh),Random.Range(yLow,yHigh));
        return;

    }
    IEnumerator SpawnCards(int answer) // problem cards the players can grab to solve the problem
    {
        //randomly select amount of cards to have on board.
        var cardAmount = Random.Range(5, 11); ; // 5-10 cards will be spawned

        var cardDisplay = new int();
        yield return new WaitForSeconds(0.5f);

        //instantiating card with correct answer
        RandomizePosition();
        var rightCard = Instantiate(cardPrefab, randPos, new Quaternion()); // spawning new card.
        rightCard.GetComponentInChildren<Card>().value = answer;

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
            RandomizePosition(); // randomizing position and spawning card
            var newCard = Instantiate(cardPrefab, randPos, new Quaternion()); // spawning new card.
            newCard.GetComponentInChildren<Card>().value = cardDisplay; // setting what is on the card
            print("Instantiating card!");
            newCard.tag = "Minigame Element"; // the card prefab won't have the tag to not be deleted.
        }
        //add all cards to array
        card = GameObject.FindGameObjectsWithTag("Minigame Element");

        //update ui
        text.text = "What is: " + number[0] + " " + problemType + " " + number[1] + "?";
    }
    public void CheckAnswer(GameObject player, int guess) // award player with points if correctly slammed right card
    {
        print(player);
        
        if(guess == answer)
        {
            text.text = "Correct! The Answer is: " + answer; // changes text to show they got it correct.
            gameScore[player.GetComponent<PlayerControls>().turnOrder]++; // player position in score array is awarded a point
            this.StartCoroutine(NewProblem()); // making new problem for player
        }
        else
        {
            //update UI on player.
        }
    }
    IEnumerator NewProblem() // procedure to put new problem on the board.
    {
        //delete all current cards
        for(int i = 0; i < card.Length; i++)
        {
            Destroy(card[i].gameObject); // destroy card in array
        }
        yield return new WaitForSeconds(1);
        MakeProblem(); // make factors and answer
        yield return new WaitForSeconds(2); // wait for two seconds so everything initalizes correctly.
        StartCoroutine(SpawnCards(answer)); // spawn cards in map
    }
}
