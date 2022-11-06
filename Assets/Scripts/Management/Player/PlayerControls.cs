//armindelmo Playermanager.cs
//the purpose of this program is to find controllers connected, then print output when found.


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerControls : MonoBehaviour
{


    //script to be called for player management
    [SerializeField]
    private GameObject managerObject;
    public PlayerManager managerScript;
    public TurnManager turnScript;

    //turns and controls
    public int turnOrder;
    public bool hasGone;

    public PlayerInput gameplayInput;
    public Controls controls;

    public bool isPressed;

    private void Awake() // can only grab things on object already
    {
        managerScript = managerObject.GetComponent<PlayerManager>(); // grab player manager
        gameplayInput = this.gameObject.GetComponent<PlayerInput>(); // grabbing player controls to turn on/off and change inputmaps
        turnScript = GameObject.FindGameObjectWithTag("Turn Manager").GetComponent<TurnManager>(); // grabs turnManager off of PlayerManager
        this.gameObject.tag = "Player"; //set player tag to "Player"
    }



    private void Start() // run methods on start
    {
        PlayerTurn(); // sets player's turn
        turnOrder = GameObject.FindGameObjectsWithTag("Player").Length; //giving player turn order

        //initalize controls class
        controls = new Controls();

    }
    void Update()
    {
        controls.boardGamePlay.DiceRoll.started += DiceRoll;

        PlayerTurn(); // sets player's turn
    }
    
    public void PlayerTurn() // check if player turn is now
    {
        //int currentTurn = managerScript.currentTurn;
        int currentTurn = turnScript.currentTurn;
        //grab all players on map, give turn order based on amount of players

        if(turnOrder != currentTurn)
        {
            gameplayInput.actions.FindActionMap("boardGamePlay").Disable(); // disable gameplay controls
            gameplayInput.actions.FindActionMap("Off").Enable(); // make it to where it changes control schemes, but otherwise we're keeping it :) 
        }
        else
        {
            gameplayInput.actions.FindActionMap("boardGamePlay").Enable(); // enable gameplay controls
            gameplayInput.actions.FindActionMap("Off").Disable(); // make it to where it changes control schemes, but otherwise we're keeping it :) 
        }

    }
    public void DiceRoll(InputAction.CallbackContext context) // run when diceroll performed
    {
        if(context.performed) // makes sure it is ONLY RAN ONCE!!!! courtesy of my boy GlenZPS
        {
            turnScript.ChangeTurn();
            hasGone = true;
        }

    }
}
