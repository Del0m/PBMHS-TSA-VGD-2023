//armindelmo Playermanager.cs
//the purpose of this program is to find controllers connected, then print output when found.


using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerControls : MonoBehaviour
{
    //stats will remain here for the time being...
    [Header("Stats")]
    public PlayerStats stat;

    //script to be called for player management
    public GameObject managerObject;
    public TurnManager turnScript;
    private MovementManager moveManage;

    [Header("Controls")]
    public PlayerInput gameplayInput;
    public Controls controls;

    private GameObject turnUI;

    [Header("Debug")]
    public float movementCooldown = 2.5f;

    private void Start() // run methods on start
    {
        try
        {
            if (moveManage == null) // check if hasn't been publicly assigned already.
            {
                moveManage = GameObject.FindGameObjectWithTag("Movement Manager").GetComponent<MovementManager>(); // grab movement manager
            }
            if (turnScript == null) // check if hasn't been publicly assigned already.
            {
                turnScript = GameObject.FindGameObjectWithTag("Turn Manager").GetComponent<TurnManager>(); // grabs turnManager off of PlayerManager
            }
        }
        catch (System.Exception)
        {
            Debug.LogError("Movement Manager or TurnManager is not present!");
        }


        gameplayInput = this.gameObject.GetComponent<PlayerInput>(); // grabbing player controls to turn on/off and change inputmaps
        this.gameObject.tag = "Player"; //set player tag to "Player"

        //initalize controls class
        controls = new Controls();

        // grabbing player controls to differentiate when its time to move or not

    }
    [HideInInspector]
    public Transform newTile; // for the purpose of updating the player to a new position!
    private void Update()
    {

        if(newTile != null)
        {
            //check distance from the new tile
            if(Vector2.Distance(newTile.position, this.transform.position) > 0){
                transform.position = Vector2.MoveTowards(this.transform.position, newTile.position, 5 * Time.deltaTime); // move to new position using DeltaTime
            }
        }
    }
    //variable for the purpose of moving
    [HideInInspector]
    public bool hasRan = false; // start off with all players being able to move.
    public void DiceRoll(InputAction.CallbackContext context) // run when player rolls dice on board
    {
        if (context.performed && hasRan == false) // makes sure its only ran once
        {
            if (turnScript.RunTurn(this.gameObject, stat.turnOrder) == true) //check to see if conditions are met on TurnManager
            {
                StartCoroutine(Moving(2)); // begin moving player
            }
        }
        IEnumerator Moving(int wait) // coroutine to move around the board.
        {
            hasRan = true; // prevent player from running coroutine again
            var diceRoll = Random.Range(1, 7); // pick a number from one to six

            var movesRemaining = diceRoll;

            //UI update
            turnScript.uiManager.UpdateDiceUI(movesRemaining);

            while (movesRemaining > 0) // keep moving player to next tile until no more moves
            {
                if (movesRemaining > 0) // check statement so program doesn't die.
                {
                    turnScript.uiManager.UpdateDiceUI(movesRemaining); // update ui for end-user

                    yield return new WaitForSeconds(movementCooldown);
                    Debug.Log("Moving.");
                    newTile = moveManage.CallTile(stat.position, 1); // moving one tile at a time

                    stat.position++; // moving position ahead
                    
                    movesRemaining--; // decrease movement till they are out of moves left.
                    yield return new WaitForSeconds(wait); // give time to move to position.
                }
            }
            turnScript.uiManager.UpdateDiceUI(movesRemaining);
            turnScript.RoundCheck(); // advance turn, see if new turn is in order.
            newTile = null; // to prevent the player from moving towards the tile in the middle of the game

            hasRan = false; // allow player to roll again, but their turn has moved, so they won't be able to.
        }
    }
}

