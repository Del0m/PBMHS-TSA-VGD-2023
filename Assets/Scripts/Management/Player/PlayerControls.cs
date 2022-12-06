//armindelmo Playermanager.cs
//the purpose of this program is to find controllers connected, then print output when found.


using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerControls : MonoBehaviour
{
    //stats will remain here for the time being...
    [Header("Stats")]
    public int position;

    //script to be called for player management
    [SerializeField]
    public GameObject managerObject;
    public TurnManager turnScript;
    public MovementManager moveManage;
    //turns and controls
    public int turnOrder;

    [Header("Controls")]
    public PlayerInput gameplayInput;
    public Controls controls;

    private GameObject turnUI;

    private void Start() // run methods on start
    {
        if(moveManage == null) // check if hasn't been publicly assigned already.
        {
            moveManage = GameObject.FindGameObjectWithTag("Movement Manager").GetComponent<MovementManager>(); // grab movement manager
        }
        if(turnScript == null) // check if hasn't been publicly assigned already.
        {
            turnScript = GameObject.FindGameObjectWithTag("Turn Manager").GetComponent<TurnManager>(); // grabs turnManager off of PlayerManager
        }
        
        gameplayInput = this.gameObject.GetComponent<PlayerInput>(); // grabbing player controls to turn on/off and change inputmaps
        this.gameObject.tag = "Player"; //set player tag to "Player"

        turnOrder = GameObject.FindGameObjectsWithTag("Player").Length; //giving player turn order

        //initalize controls class
        controls = new Controls();  

        //make area to set start position. (this shouldn't be done here)

        //turnui initaliation
        //turnUI = GameObject.Find("Moves Left");
        // useless for the time being..

    }
    
    //variable for the purpose of moving
    bool hasRan = false;
    public void DiceRoll(InputAction.CallbackContext context) // run when diceroll performed
    {
        if(context.performed && hasRan == false) // makes sure its only ran once
        {
            turnScript.RunTurn(this.gameObject); // adds self to turnmanager function.
            //this will recurse back to the player, it just checks directly for whose turn it is without messing with the controls
        }
    }
    public IEnumerator Moving(int wait)
    {
        hasRan = true;
        var diceRoll = Random.Range(1, 7);

        var movesRemaining = diceRoll;
        while(movesRemaining > 0) // keep moving player to next tile until no more moves
        {
            if(movesRemaining > 0) // check statement so program doesn't die.
            {
                Debug.Log("Moving.");
                var newTile = moveManage.CallTile(position, 1); // moving one tile at a time
                position++; // moving position ahead

                while(this.transform.position.x != newTile.transform.position.x && this.transform.position.y == newTile.transform.position.y) // keep user moving until they get there.
                {
                    //need to find better solution, really slow while moving.
                    this.transform.position = Vector2.MoveTowards(this.transform.position, newTile.position, 2 * Time.deltaTime); // move to new position using DeltaTime
                }

                movesRemaining--; // decrease movement till they are out of moves left.
                yield return new WaitForSeconds(wait); // give time to move to position.
            }
        }
        hasRan = false;
    }
}
