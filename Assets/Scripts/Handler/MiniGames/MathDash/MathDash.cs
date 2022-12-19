//armin delmo; Mathdash.cs; this is the math dash minigame

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathDash : GameHandler
{
    //problem variables
    public int[] number = new int[2]; // numbers that will be used for the problems.
    public char problemType;
    static int answer;
    public GameObject cardPrefab;

    public GameObject[] card;

    private Vector2 randPos;
    [SerializeField]
    static int[] gameScore = { 0,0,0,0 };
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(TeleportPlayers()); // teleport players to the minigame.
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
        var xLow = GameObject.Find("Outer Left").transform.position.x+1;
        var xHigh = GameObject.Find("Outer Right").transform.position.x-1;

        var yLow = GameObject.Find("Outer Bottom").transform.position.y+3;
        var yHigh = GameObject.Find("Outer Top").transform.position.y-3;

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
        rightCard.GetComponent<Card>().value = answer;

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
                    cardDisplay = answer * randomized[0];
                    //instantiate card with answer
                    break;
            }
            //spawn in object in random area in minigame arena.
            RandomizePosition(); // randomizing position and spawning card
            var newCard = Instantiate(cardPrefab, randPos, new Quaternion()); // spawning new card.
            newCard.GetComponent<Card>().value = cardDisplay; // setting what is on the card
            print("Instantiating card!");
        }

    }
    public void CheckAnswer(GameObject player, int guess) // award player with points if correctly slammed right card
    {
        Debug.Log(guess + " " + answer);
        print(player);
        
        if(guess == answer)
        {
            //update UI in here
            gameScore[player.GetComponent<PlayerControls>().turnOrder]++; // player position in score array is awarded a point
            Debug.Log("correct!");
            StartCoroutine(NewProblem()); // making new problem for player
        }
        else
        {
            //update UI on player.
            Debug.Log("incorrect!");
        }
    }
    IEnumerator NewProblem() // procedure to put new problem on the board.
    {
        MakeProblem(); // make factors and answer
        yield return new WaitForSeconds(2); // wait for two seconds so everything initalizes correctly.
        StartCoroutine(SpawnCards(answer)); // spawn cards in map
    }
}
