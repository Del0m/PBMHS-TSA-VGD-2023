//armindelmo Playermanager.cs
//the purpose of this program is to find controllers connected, then print output when found.


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerControls : MonoBehaviour
{
    public bool isPressed;

    public PlayerManager managerScript;

    //turns and controls
    public int turnOrder;
    public PlayerInput gameplayInput;
    private void Awake() // for purpose of controls / turns
    {
        PlayerTurn(); // sets player's turn
        gameplayInput = this.gameObject.GetComponent<PlayerInput>();

    }
    

    
    private void Start() // depreciated for the moment
    {
       managerScript = GameObject.FindGameObjectWithTag("Player Manager").GetComponent<PlayerManager>(); //find manager script

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
        //grab all players on map, give turn order based on amount of players
        turnOrder = GameObject.FindGameObjectsWithTag("Player").Length;

        if(turnOrder != managerScript.turnManager.currentTurn)
        {
            gameplayInput.DeactivateInput(); // make it to where it changes control schemes, but otherwise we're keeping it :) 
        }

    }

    private void OnDestroy() // updates player manager when player disappears
    {
        managerScript.GetPlayers();
    }
}
