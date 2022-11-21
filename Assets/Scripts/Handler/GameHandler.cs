//armin delmo, 11/13/22. Purpose of script is to handle minigame startup, grabbing players, starting game, etc.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    //arrays
    private GameObject[] spawns;
    private GameObject[] players;

    //user choices
    public string input;

    //children objects
    public Camera miniCam;

    //turn
    private int gameOrder = 1;

    // Start is called before the first frame update
    void Start()
    {
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
            GradeMove(input); // check trivia move for validity
            return true;
        }

        gameOrder++;
        if (gameOrder > players.Length) { gameOrder = 1; } // quickly check if turns need to be looped back

        return false;
    }
    public virtual void GradeMove(string move)
    {
        //holds the grading procedure for the move; ONLY TO BE EDITED IN THE CHILDREN SCRIPTS
    }

}
