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

    //scripts / objects to be called for player management
    public GameObject managerObject;
    public PlayerManager managerScript;

    //turns and controls
    public int turnOrder;
    public PlayerInput gameplayInput;


    private void Awake() // for purpose of controls / turns
    {
        Debug.Log("Testing");
        PlayerTurn(); // sets player's turn
        gameplayInput = this.gameObject.GetComponent<PlayerInput>(); // grabbing player controls to turn on/off and change inputmaps

        if(managerObject == null)
        {
            Debug.Log("Grabbing object");
            managerObject = GameObject.Find("Player Manager"); //find manager script
           
        }
        
    }



    private void Start() // depreciated for the moment
    {
        
        

        managerScript = managerObject.GetComponent<PlayerManager>();
        managerScript.GetPlayers(); // update player array at start
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
    
    public void PlayerTurn()
    {
        var currentTurn = managerScript.turnManager.currentTurn;
        print("The current turn is: " + currentTurn);
        //grab all players on map, give turn order based on amount of players
        turnOrder = GameObject.FindGameObjectsWithTag("Player").Length;

        if(turnOrder != 2)
        {
            gameplayInput.actions.FindActionMap("Gameplay").Disable(); // disable gameplay controls
            gameplayInput.actions.FindActionMap("Off").Enable(); // make it to where it changes control schemes, but otherwise we're keeping it :) 
        }

    }

    private void OnDestroy() // updates player manager when player disappears
    {
        managerScript.GetPlayers();
    }
}
