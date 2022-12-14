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
    void SpawnCards() // problem cards the players can grab to solve the problem
    {

    }
    void NewProblem()
    {

    }
}
