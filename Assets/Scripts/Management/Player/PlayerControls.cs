//armindelmo Playermanager.cs
//the purpose of this program is to find controllers connected, then print output when found.


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerControls : MonoBehaviour
{
    //controls
    public bool isPressed;

    //script to be called for player management
    [SerializeField]
    private GameObject managerObject;
    public PlayerManager managerScript;

    //turns and controls
    public int turnOrder;
    public PlayerInput gameplayInput;


    private void Awake() // can only grab things on object already
    {
        managerScript = managerObject.GetComponent<PlayerManager>(); // grab player manager
        gameplayInput = this.gameObject.GetComponent<PlayerInput>(); // grabbing player controls to turn on/off and change inputmaps

        this.gameObject.tag = "Player"; //set player tag to "Player"
    }



    private void Start() // run methods on start
    {
        PlayerTurn(); // sets player's turn
        turnOrder = GameObject.FindGameObjectsWithTag("Player").Length; //giving player turn order

        managerScript.GetPlayers(); //updating player manager
    }
    void Update()
    {
        //detect if player has pressed key
        if (isPressed != false)
        {
            Debug.Log("Button is Pressed on gamepad!");
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 1, this.transform.position.z); // make new player
        }
    }
    public void OnPress(InputAction.CallbackContext ctx) => isPressed = ctx.ReadValueAsButton(); // returns true when button pressed
    
    public void PlayerTurn() // check if player turn is now
    {
        var currentTurn = managerScript.currentTurn;
        print("The current turn is: " + currentTurn);
        //grab all players on map, give turn order based on amount of players

        if(turnOrder != currentTurn)
        {
            gameplayInput.actions.FindActionMap("Gameplay").Disable(); // disable gameplay controls
            gameplayInput.actions.FindActionMap("Off").Enable(); // make it to where it changes control schemes, but otherwise we're keeping it :) 
        }

    }
    private int StartUp() // due to prefab limitations, start and awake cannot work on players
    {
        int finish = 1;

        if(finish != 1)
        {
            //find player manager, run essentials at startup
            PlayerTurn();
        }
        //grabbing player manager
        return finish;
    }
}
