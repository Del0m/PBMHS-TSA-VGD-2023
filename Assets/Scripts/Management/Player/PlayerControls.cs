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


        //initalizing inital position
        var val = turnOrder - 1;
        var initalSpot = GameObject.Find("PlayerSpawn" + val);
        this.gameObject.transform.position = new Vector3(initalSpot.transform.position.x,this.transform.position.y+1,initalSpot.transform.position.z);

        var playerMovementScript = this.gameObject.GetComponent<PlayerMovement>();
        playerMovementScript.norm = this.gameObject.transform.position;

    }
    void Update()
    {
        //control update functions
        //controls.miniGamePlay.MiniGameTest.started += MiniGameTest; // test control for minigame mode
        //controls.boardGamePlay.DiceRoll.started += DiceRoll; // test control for boardgame mode
        if(GameObject.FindGameObjectWithTag("Mini Game Manager").GetComponent<MiniGameManager>().hasStarted == false)
        {
            gameplayInput.SwitchCurrentActionMap("boardGamePlay");
        }
        else
        {
            gameplayInput.SwitchCurrentActionMap("miniGamePlay");
        }
        Debug.Log(gameplayInput.currentActionMap);
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
    public void FinishTurn()
    {
        turnScript.currentTurn++; // advance turn
        PlayerTurn();
    }
    //
    public void TriviaGameInput(InputAction.CallbackContext context) // run when minigametest performed
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
