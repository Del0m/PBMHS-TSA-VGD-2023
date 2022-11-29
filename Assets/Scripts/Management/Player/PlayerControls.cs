//armindelmo Playermanager.cs
//the purpose of this program is to find controllers connected, then print output when found.


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerControls : MonoBehaviour
{
    //stats will remain here for the time being...
    [Header("Stats")]
    public int position;

    //script to be called for player management
    [SerializeField]
    private GameObject managerObject;
    public PlayerManager managerScript;
    private TurnManager turnScript;
    private MovementManager moveManage;
    //turns and controls
    public int turnOrder;

    [Header("Controls")]
    public PlayerInput gameplayInput;
    public Controls controls;

    private void Start() // run methods on start
    {
        managerScript = GameObject.FindGameObjectWithTag("Player Manager").GetComponent<PlayerManager>();// grab player manager
        moveManage = GameObject.FindGameObjectWithTag("Movement Manager").GetComponent<MovementManager>(); // grab movement manager
        gameplayInput = this.gameObject.GetComponent<PlayerInput>(); // grabbing player controls to turn on/off and change inputmaps
        turnScript = GameObject.FindGameObjectWithTag("Turn Manager").GetComponent<TurnManager>(); // grabs turnManager off of PlayerManager
        this.gameObject.tag = "Player"; //set player tag to "Player"

        turnOrder = GameObject.FindGameObjectsWithTag("Player").Length; //giving player turn order

        //initalize controls class
        controls = new Controls();
        PlayerTurn();

        //setting start position
        this.transform.position = moveManage.CallTile(position, 0).position;

    }
    void Update()
    {
        //control update functions
        //controls.miniGamePlay.MiniGameTest.started += MiniGameTest; // test control for minigame mode
        //controls.boardGamePlay.DiceRoll.started += DiceRoll; // test control for boardgame mode
        Debug.Log(gameplayInput.currentActionMap);
        if(turnScript.currentTurn == turnOrder)
        {
            PlayerTurn();
        }

    }
    
    public void PlayerTurn() // check if player turn is now
    {
        int currentTurn = turnScript.currentTurn; //grab all players on map, give turn order based on amount of players
        Debug.Log("Current Turn: " + currentTurn + " Turn Order: " + turnOrder);

        if (turnOrder != currentTurn)
        {
            Debug.Log("Turning off action...");
            gameplayInput.SwitchCurrentActionMap("Off"); // disable gameplay controls

        }
        else
        {
            gameplayInput.SwitchCurrentActionMap("boardGamePlay"); // enable gameplay controls
        }
    }
    public void FinishTurn()
    {
        Debug.Log("Finished Turn!");
        turnScript.currentTurn++; // advance turn
        PlayerTurn();
    }
    //variable for the purpose of moving
    bool hasRan = false;
    public void DiceRoll(InputAction.CallbackContext context) // run when diceroll performed
    {
        if(context.performed && hasRan == false) // makes sure its only ran once
        {
            StartCoroutine(Moving(2));
        }
    }
    IEnumerator Moving(int wait)
    {
        hasRan = true;
        var diceRoll = Random.Range(1, 7);

        var movesRemaining = diceRoll;
        while(movesRemaining > 0) // keep moving player to next tile until no more moves
        {
            Debug.Log("Moving.");
            var newTile = moveManage.CallTile(position, 1); // moving one tile at a time
            position++; // moving position ahead
            this.transform.position = newTile.position; // teleport to new position
            movesRemaining--; // decrease movement till they are out of moves left.
            yield return new WaitForSeconds(wait); // wait a certain amount of time until charting the next move.
        }
        hasRan = false;
        FinishTurn(); // end turn, send to next player
    }
    public void TriviaGameInput(InputAction.CallbackContext context) // run when tirivalinput is performed
    {
        if (context.performed) // makes sure it is ONLY RAN ONCE!!!! courtesy of my boy GlenZPS
        {
            var input = "null";

            // call minigame
            var minigame = GameObject.FindGameObjectWithTag("Minigame").GetComponent<GameHandler>();

            switch(context.action.name) // multiple choice input for trivia game
            {
                case "TriviaInput1":
                    input = "A";
                    Debug.Log("User " + turnOrder + "has inputted " + input);
                    break;

                case "TriviaInput2":
                    input = "B";
                    Debug.Log("User " + turnOrder + "has inputted " + input);
                    break;

                case "TriviaInput3":
                    input = "C";
                    Debug.Log("User " + turnOrder + "has inputted " + input);
                    break;

                case "TriviaInput4":
                    input = "D";
                    Debug.Log("User " + turnOrder + "has inputted " + input);
                    break;
            }
            if(GameObject.FindGameObjectWithTag("Minigame") != null)
            {
                minigame.InputChoice(input, turnOrder);
            }

        }

    }
    public void PlayerMiniGameMode(bool inMiniGame)
    {
        if(inMiniGame == true)
        {
            gameplayInput.SwitchCurrentActionMap("miniGamePlay");
        }
        else
        {
            Debug.Log("enabling boardgameplay!");
            gameplayInput.SwitchCurrentActionMap("boardGamePlay");
        }
    }
}
