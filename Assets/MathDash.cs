//armin delmo, math dash game for VGD 2022. Guantanamo Bay cannot get me to work on this PoS again...
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; // for sensing input
using TMPro; // for text mesh modification
public class MathDash : GameHandler
{
    string rightMove = "A";
    private int[] numbers = new int[2];


    //UI definitions
    public TextMeshProUGUI[] question;

    private void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        CreateQuestion(); // start minigame
    }

    private int[] CreateQuestion() // uses math formula to return 4 choices
    {
        //math formula to create questions
        var questionType = Random.Range(0, 3); // 0 addition, 1 subtraction, 2 mult, 3 division
        var questionsPrompts = new int[4];
        var problemType = "null";

        switch (questionType) // question type deciding what math question to throw.
        {
            case 0: // addition
                numbers[0] = Random.Range(0, 200); 
                numbers[1] = Random.Range(0, 200);

                var addition = new int[] // first entry is correct, the rest are wrong
                {
                    numbers[0] + numbers[1],
                    numbers[0] - Random.Range(2,10) + numbers[1],
                    numbers[0] / 2 + numbers[1],
                    numbers[1] - numbers[0]

                };
                //two numbers that will be given to player to decipher
                problemType = "+";

                questionsPrompts = addition;
                break;
            case 1: // subtraction
                numbers[0] = Random.Range(0, 200);
                numbers[1] = Random.Range(0, 200);

                var subtraction = new int[] // first entry is correct, the rest are wrong
                {
                    numbers[0] - numbers[1],
                    numbers[0] + Random.Range(2,10) - numbers[1],
                    numbers[0] / 2 - numbers[1],
                    numbers[0] - numbers[1]

                };
                //two numbers that will be given to player to decipher
                
                //numbers[1] = numberB;
                problemType = "-";
                questionsPrompts = subtraction;
                break;
            case 2: // multiplication
                numbers[0] = Random.Range(1, 14);
                numbers[1] = Random.Range(1, 14);

                var multiplication = new int[] // first entry is correct, the rest are wrong
                {
                    numbers[0] * numbers[1],
                    numbers[0] * Random.Range(2,10) - numbers[1],
                    numbers[0] * 2 - numbers[1],
                    numbers[0] / numbers[1]

                };
                //two numbers that will be given to player to decipher
                //numbers[0] = numberC;
                //numbers[1] = numberD;
                problemType = "x";
                questionsPrompts = multiplication;
                break;
                
            case 3: // division 
                numbers[0] = Random.Range(15, 100);
                numbers[1] = Random.Range(1, 10);

                //while loop to get a whole answer 
                while(numbers[0] / numbers[1] % 1 != 0)
                {
                    numbers[0] = Random.Range(15, 100);
                    numbers[1] = Random.Range(1, 10);
                }

                var division = new int[] // first entry is correct, the rest are wrong
                {
                    numbers[0] / numbers[1],
                    numbers[0] / 2 * numbers[1],
                    numbers[0] * 2 / numbers[1],
                    numbers[1] / numbers[0]

                };
                //two numbers that will be given to player to decipher
                // numbers[0] = numberE;
                //numbers[1] = numberF;
                problemType = "/";
                questionsPrompts = division;
                break;
        }
        SortQuestions(questionsPrompts, numbers, problemType);
        return questionsPrompts;
    }

    private int[] SortQuestions(int[] answers, int[] addends, string math) // sorts the four questions 
    {
        print("MATH DASH " + "Creating questions");
        var sortedQuestions = new int[4]; // questions that will show up on the UI
        var sortParameter = new int[4];

        //randomizer formula for sorting questions
        for(int i = 0; i < 3; i++)
        {
            var randInt = Random.Range(0, 4);

            for(int j = 0; j < i;)
            {
                while(randInt == sortParameter[j])
                {
                    randInt = Random.Range(0, 3);
                }
                j++;
            }
            sortParameter[i] = randInt;

        }
        print(sortParameter[0] + " " + sortParameter[1] + " " + sortParameter[2] + " " + sortParameter[3]);
        for (int i = 0; i < 3; i++) // randomizing each question's position.
        {
            sortedQuestions[sortParameter[i]] = answers[i];
        }
        var loop = 0;

        while(sortedQuestions[loop] != answers[0]) // finding correct answer
        {
            loop++;
        }
        if(loop >= 4) //this recursion is only a front to my awful coding skills, this makes my code not crap the bed.
        {
            SortQuestions(answers, addends, math); // recurse to prevent error
            return sortedQuestions;
        }

        switch (loop) // assign correct question to right move
        {
            case 0:
                rightMove = "A";
                break;
            case 1:
                rightMove = "B";
                break;
            case 2:
                rightMove = "C";
                break;
            case 3:
                rightMove = "D";
                break;
        }
        Debug.LogWarning("The correct answer is: " + rightMove);
        ProgramUI(sortedQuestions, numbers, math); // update ui for questions
        return sortedQuestions; //useless. for now :)
    }
    private void ProgramUI(int[] entry, int[] numbers, string math) // adds all the arrays onto a UI
    {
        //collects ui elements from minigame object
        var prompt = GameObject.FindGameObjectWithTag("Minigame Question");


        var player = gameOrder; // player turn order

        //for loop to change all questions to diff names
        for(int i = 0; i < question.Length; i++)
        {
            question[i].text = i+1 + ": " + entry[i]; // updates answer on UI for user to see.
        }
        prompt.GetComponent<TextMeshProUGUI>().text = numbers[0] + " " + math + " " + numbers[1];

        //program ui to show current person's turn
        var plrTurn = GameObject.FindGameObjectWithTag("Minigame Turn");

        plrTurn.GetComponent<TextMeshProUGUI>().text = "Player " + player + "'s turn!";


    }

    public override void GradeMove(string move, int turn) // checks move if its valid
    {
       if(move == rightMove) // correct move made
        {
            Debug.Log("Correct!");
            score[turn-1]++; // add score
            CreateQuestion(); // make new question
            EndGame(score[turn - 1]); // check to see if game needs to end


        }
        else
        {
            Debug.Log("Incorrect!");
            CreateQuestion();
        }

    }
    // partly depreciated until this function can actually trigger stuff on MiniGameManager
    private void EndGame(int score)
    {
        print("Recorded score is: " + score);
        if (score >= 5)
        {
            miniManager.EndMiniGame();
            miniManager.hasStarted = false;
            for (int i = 0; i < players.Length; i++)
            {
                if(players.Length == 0)
                {
                    break;
                }
                players[i].GetComponent<PlayerInput>().SwitchCurrentActionMap("boardGamePlay");
            }
            Debug.Log("Game is Done!");
        }
    }

    public override void JumpStart()
    {
        base.JumpStart();

       /*
        * for (int i = 0; i < players.Length; i++)
        {
            // change controls on all the players
            players[i].GetComponent<PlayerControls>().PlayerMiniGameMode(true);
        }
       */
        CreateQuestion();
    }

}
