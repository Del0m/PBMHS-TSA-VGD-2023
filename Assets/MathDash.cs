//armin delmo, math dash game for VGD 2022. Guantanamo Bay cannot get me to work on this PoS again...
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; // for sensing input
public class MathDash : GameHandler
{
    string rightMove = "A";
    private int[] numbers = new int[2];
    //player values
    private int[] score = new int[4];

    private void Start()
    {
        CreateQuestion(); // start minigame
    }
    private int[] CreateQuestion() // uses math formula to return 4 choices
    {
        //math formula to create questions
        var questionType = Random.Range(0, 3); // 0 addition, 1 subtraction, 2 mult, 3 division
        var questionsPrompts = new int[5];
        var problemType = "null";

        switch (questionType) // question type deciding what math question to throw.
        {
            case 0: // addition
                numbers[0] = Random.Range(0, 200); //numbers[0] = numberA;
                numbers[1] = Random.Range(0, 200);// numbers[1] = numberB;

                var addition = new int[] // first entry is correct, the rest are wrong
                {
                    numbers[0] + numbers[1],
                    numbers[0] - Random.Range(2,10) + numbers[1],
                    numbers[0] / 2 + numbers[1],
                    numbers[1] - numbers[0],
                    numbers[0] + numbers[1] // correct answer

                };
                //two numbers that will be given to player to decipher
                problemType = "addition";

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
                    numbers[0] - numbers[1],
                    numbers[0] - numbers[1] // correct answer

                };
                //two numbers that will be given to player to decipher
                
                //numbers[1] = numberB;
                problemType = "subtraction";
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
                    numbers[0] / numbers[1],
                    numbers[0] * numbers[1] // correct answer

                };
                //two numbers that will be given to player to decipher
                //numbers[0] = numberC;
                //numbers[1] = numberD;
                problemType = "multiplication";
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
                    numbers[1] / numbers[0],
                    numbers[0] / numbers[1] // correct answer

                };
                //two numbers that will be given to player to decipher
                // numbers[0] = numberE;
                //numbers[1] = numberF;
                problemType = "division";
                questionsPrompts = division;
                break;
        }
        SortQuestions(questionsPrompts, numbers, problemType);
        return questionsPrompts;
    }

    private int[] SortQuestions(int[] answers, int[] addends, string math) // sorts the four questions 
    {
        var sortedQuestions = new int[4]; // questions that will show up on the UI
        var sortParameter = new int[4];

        //randomizer formula for sorting questions
        for(int i = 0; i <= 3; i++)
        {
            var randInt = Random.Range(0, 3);

            for(int j = 0; j < i; j++)
            {
                while(randInt == sortParameter[j])
                {
                    randInt = Random.Range(0, 3);
                }
            }
            sortParameter[i] = randInt;
        }  
        
        for(int i = 0; i < 3; i++) // randomizing each question's position.
        {
            sortedQuestions[sortParameter[i]] = answers[i];
        }
        var loop = 0;

        while(sortedQuestions[loop] != answers[0]) // finding correct answer
        {
            loop++;
        }
        if(loop >= 4)
        {
            SortQuestions(answers, addends, math); // recurse to prevent error
            return sortedQuestions;
        }

        switch (loop)
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
        //ProgramUI();
        return sortedQuestions;
    }
    public override void CollectUI()
    {
        //collects ui elements from minigame object
        var canvas = GameObject.FindGameObjectWithTag("Minigame UI");
        GameObject.Find
    }
    public override void ProgramUI() // adds all the arrays onto a UI
    {

    }

    public override void GradeMove(string move, int turn)
    {
       if(move == rightMove)
        {
            Debug.Log("Correct!");
            score[turn-1]++;
            CreateQuestion();

        }
        else
        {
            Debug.Log("Incorrect!");
            CreateQuestion();
        }

    }

}
