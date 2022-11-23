using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; // for sensing input
public class MathDash : GameHandler
{
    string rightMove = "A";
    int[] numbers;
    //player values
    private int[] score = new int[4];
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

             // check every frame to see if correct move is made
    }
    private int[] CreateQuestion() // uses math formula to return 4 choices
    {
        //math formula to create questions
        var questionType = Random.Range(0, 3); // 0 addition, 1 subtraction, 2 mult, 3 division
        var questionsPrompts = new int[5];
        switch(questionType) // question type deciding what math question to throw.
        {
            case 0: // addition
                var numberA = Random.Range(0, 200); //numbers[0] = numberA;
                var numberB = Random.Range(0, 200);// numbers[1] = numberB;

                var addition = new int[] // first entry is correct, the rest are wrong
                {
                    numberA + numberB,
                    numberA - Random.Range(2,10) + numberB,
                    numberA / 2 + numberB,
                    numberB - numberA,
                    numberA + numberB // correct answer

                };
                //two numbers that will be given to player to decipher



                questionsPrompts = addition;
                break;
            case 1: // subtraction
                numberA = Random.Range(0, 200);
                numberB = Random.Range(0, 200);

                var subtraction = new int[] // first entry is correct, the rest are wrong
                {
                    numberA - numberB,
                    numberA + Random.Range(2,10) - numberB,
                    numberA / 2 - numberB,
                    numberB - numberA,
                    numberB - numberA // correct answer

                };
                //two numbers that will be given to player to decipher
               // numbers[0] = numberA;
                //numbers[1] = numberB;

                questionsPrompts = subtraction;
                break;
            case 2: // multiplication
                var numberC = Random.Range(1, 14);
                var numberD = Random.Range(1, 14);
                var multiplication = new int[] // first entry is correct, the rest are wrong
                {
                    numberC * numberD,
                    numberC * Random.Range(2,10) - numberD,
                    numberC * 2 - numberD,
                    numberC / numberD,
                    numberC * numberD // correct answer

                };
                //two numbers that will be given to player to decipher
                //numbers[0] = numberC;
                //numbers[1] = numberD;

                questionsPrompts = multiplication;
                break;
            case 3: // division 
                var numberE = Random.Range(15, 100);
                var numberF = Random.Range(1, 10);

                //while loop to get a whole answer 
                while(numberE/numberF % 1 != 0)
                {
                    numberE = Random.Range(15, 100);
                    numberF = Random.Range(1, 10);
                }

                var division = new int[] // first entry is correct, the rest are wrong
                {
                    numberE / numberF,
                    numberE / 2 * numberF,
                    numberE * 2 / numberF,
                    numberF / numberE,
                    numberE / numberF // correct answer

                };
                //two numbers that will be given to player to decipher
               // numbers[0] = numberE;
                //numbers[1] = numberF;

                questionsPrompts = division;
                break;
        }
        SortQuestions(questionsPrompts, numbers);
        return questionsPrompts;
    }

    private int[] SortQuestions(int[] answers, int[] addends) // sorts the four questions 
    {
        var sortedQuestions = new int[4]; // questions that will show up on the UI
        var random = new int[4] { 3, 2, 1, 0 };
        for(int i = 0; i < 3; i++) // randomizing each question's position.
        {
            sortedQuestions[random[i]] = answers[i];
        }
        var loop = 0;

        while(sortedQuestions[loop] != answers[0]) // finding correct answer
        {
            Debug.LogError(sortedQuestions[loop] + " and " + answers[0]);
            loop++;
        }
        

        switch(loop)
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
        return sortedQuestions;
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
