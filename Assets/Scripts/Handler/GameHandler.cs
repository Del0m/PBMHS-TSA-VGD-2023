//armin delmo, 11/13/22. Purpose of script is to handle minigame startup, grabbing players, starting game, etc.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    //arrays
    private GameObject[] spawns;
    public GameObject[] players;

    //user choices
    public string input;

    //children objects
    public Camera miniCam;

    //turn
    public int gameOrder = 1;

    //ouytside scripts
    public MiniGameManager miniManager;

    //player values
    public int[] score = new int[4];

    // Start is called before the first frame update
    void Start()
    {
        miniManager = GameObject.FindGameObjectWithTag("Mini Game Manager").GetComponent<MiniGameManager>();

        players = GameObject.FindGameObjectsWithTag("Player"); // find all players

        spawns = GameObject.FindGameObjectsWithTag("Minigame Spawn"); // spawn for player to put in if need be.

        //calling children components
        miniCam = this.gameObject.GetComponentInChildren<Camera>(); // should grab camera off of Minigame View.
    }

    public void InputChoice(string choice, int turnOrder) // input for trivia-like games, T/F, multiple choice, etc.
    {
        input = choice; // change current input, i'll do something fancier l8r

        //checking if turn was made durinng right turn
        CheckResult(turnOrder, gameOrder);
    }

    public bool CheckResult(int playerTurn, int currentTurn) // checks to see if turn was allowed to happen, and then send forward result 
    {
        players = GameObject.FindGameObjectsWithTag("Player"); // find all players

        if (playerTurn == currentTurn)
        {
            gameOrder++;
            if (gameOrder > players.Length) { gameOrder = 1; } // quickly check if turns need to be looped back
            GradeMove(input, gameOrder); // check trivia move for validity
            return true;
        }

        if (gameOrder > players.Length) { gameOrder = 1; } // quickly check if turns need to be looped back

        return false;
    }
    public virtual void GradeMove(string move, int turn)
    {
        //holds the grading procedure for the move; ONLY TO BE EDITED IN THE CHILDREN SCRIPTS
    }
    public  virtual void JumpStart() // to be used to start the questions for the game.
    {
        score = new int[4] { 0, 0, 0, 0 }; // setting arrays with base values
    }
    //timer for the player; will change gameorder when timer runs out; to be used in any minigame that needs it
    public bool Timer(double timeRemaining)
    {
        //using time.deltaTime to how long user has left
        while (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime; //deltatime tracks time fairly easily
        }

        if (timeRemaining < 0) // check if time is up after while loop
        {
            Debug.Log("Times up!");
            return true; 
        }
        return false; // return false in case of error
    }

}
