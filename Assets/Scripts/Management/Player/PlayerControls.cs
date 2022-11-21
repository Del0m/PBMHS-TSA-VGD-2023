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
    public int playerScore;

    //script to be called for player management
    [SerializeField]
    private GameObject managerObject;
    public PlayerManager managerScript;
    public TurnManager turnScript;

    //turns and controls
    public int turnOrder;

    [Header("Controls")]
    public PlayerInput gameplayInput;
    public Controls controls;
    private void Awake() // can only grab things on object already
    {
        managerScript = managerObject.GetComponent<PlayerManager>(); // grab player manager
        gameplayInput = this.gameObject.GetComponent<PlayerInput>(); // grabbing player controls to turn on/off and change inputmaps
        turnScript = GameObject.FindGameObjectWithTag("Turn Manager").GetComponent<TurnManager>(); // grabs turnManager off of PlayerManager
        this.gameObject.tag = "Player"; //set player tag to "Player"
    }



    private void Start() // run methods on start
    {
        turnOrder = GameObject.FindGameObjectsWithTag("Player").Length; //giving player turn order

        //initalize controls class
        controls = new Controls();

    }
    void Update()
    {
        //control update functions
        //controls.miniGamePlay.MiniGameTest.started += MiniGameTest; // test control for minigame mode
        //controls.boardGamePlay.DiceRoll.started += DiceRoll; // test control for boardgame mode
    }
    
    public void PlayerTurn() // check if player turn is now
    {
        Debug.Log("running PlayerTurn on player " + turnOrder);
        int currentTurn = turnScript.currentTurn;

        Debug.Log(currentTurn);
        //grab all players on map, give turn order based on amount of players

        if(turnOrder != currentTurn)
        {
            gameplayInput.actions.FindActionMap("boardGamePlay").Disable(); // disable gameplay controls
        }
        else
        {
            gameplayInput.actions.FindActionMap("boardGamePlay").Enable(); // enable gameplay controls
        }

    }
    public void DiceRoll(InputAction.CallbackContext context) // run when diceroll performed
    {
        if(context.performed) // makes sure it is ONLY RAN ONCE!!!! courtesy of my boy GlenZPS
        {
            Debug.Log("Performing DiceRoll!");
            turnScript.currentTurn++;
            PlayerTurn();
        }

    }
    //
    public void TriviaGameInput(InputAction.CallbackContext context) // run when minigametest performed
    {
        Debug.Log("Input Performed!");
        Debug.Log(context.action.name);
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
            minigame.InputChoice(input, turnOrder);
        }

    }
    public void PlayerMiniGameMode(bool inMiniGame)
    {
        if(inMiniGame == true)
        {
            gameplayInput.actions.FindActionMap("boardGamePlay").Disable(); // disable boardgame controls
            gameplayInput.actions.FindActionMap("miniGamePlay").Enable(); // make it to where it changes control schemes, but otherwise we're keeping it :) 
            Debug.Log("enabling minigameplay!");
        }
        else
        {
            Debug.Log("enabling boardgameplay!");
            gameplayInput.actions.FindActionMap("miniGamePlay").Disable(); // disable minigame controls
            gameplayInput.actions.FindActionMap("boardGamePlay").Enable(); // make it to where it changes control schemes, but otherwise we're keeping it :) 
        }
    }
}
