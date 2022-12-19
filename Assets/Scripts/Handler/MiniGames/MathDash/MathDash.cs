//armin delmo; Mathdash.cs; this is the math dash minigame

using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class MathDash : GameHandler
{
    //problem variables
    public int[] number = new int[2]; // numbers that will be used for the problems.
    public char problemType;
    public int answer;
    public GameObject cardPrefab;

    public GameObject[] card;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(TeleportPlayers()); // teleport players to the minigame.
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

                //making nums for problem
                number[0] = Random.Range(0, 30);
                number[1] = Random.Range(0, 30);

                //running loop to ensure whole number is gotten for players
                while (number[0] % number[1] != 0)
                {
                    number[0] = Random.Range(0, 30);
                    number[1] = Random.Range(0, 30);
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
        var rightCard = Random.Range(0,cardAmount - 1); // in array

        var cardDisplay = new int();
        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < cardAmount; i++) // for loop to spawn in cards with random values
        {
            var rand = Random.Range(0, 5);
            switch (rand) // switch statement to decide what is on the card.
            {
                case 0:
                    cardDisplay = answer + Random.Range(1, 11);
                    //instantiate card with +1-10
                    break;
                case 1:
                    cardDisplay = -answer;
                    //instantiate card with answer negative
                    break;
                case 2:
                    cardDisplay = answer - Random.Range(1, 11);
                    //instantiate card with answer - 10
                    break;
                case 3:
                    cardDisplay = answer - Random.Range(1, 6);
                    //instantiate card with answer +- (1,5)
                    break;
                case 4:
                    cardDisplay = answer;
                    //instantiate card with answer
                    break;
            }
            //spawn in object in random area in minigame arena.
            //Instantiate(cardPrefab)
        }

    }
    public void CheckAnswer(GameObject player, int guess) // award player with points if correctly slammed right card
    {
        if(guess == answer)
        {
            //update UI in here
            gameScore[player.GetComponent<PlayerControls>().turnOrder]++; // player position in score array is awarded a point
            Debug.Log("correct!");
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
    }
}
