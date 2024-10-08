//armindelmo Playermanager.cs
//the purpose of this program is to find controllers connected, then print output when found.


using JetBrains.Annotations;
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
    public TurnManager turnScript;
    public MovementManager moveManage;
    [HideInInspector]
    public PlayerMovement movement;

    [Header("Controls")]
    public PlayerInput gameplayInput;
    public Controls controls;

    private GameObject turnUI;

    public GameObject pauseMenu; // to be passed from player maanger

    public CameraControl cam;

    [Header("Debug")]

    public int inZoom = 20;

    public int outZoom = 30;

    public int cameraSpeed = 10;

    public float movementCooldown = 0.5f;


    private void Start() // run methods on start
    {
        movement = this.GetComponent<PlayerMovement>();

        gameplayInput = this.gameObject.GetComponent<PlayerInput>(); // grabbing player controls to turn on/off and change inputmaps
        this.gameObject.tag = "Player"; //set player tag to "Player"

        //initalize controls class
        controls = new Controls();

        // grabbing player controls to differentiate when its time to move or not
        try
        {
            turnScript = GameObject.FindGameObjectWithTag("Turn Manager").GetComponent<TurnManager>();
        }
        catch (System.Exception)
        {

            throw;
        }
    }
    public Transform newTile; // for the purpose of updating the player to a new position!

    //variable for the purpose of moving
    public bool hasRan = false; // start off with all players being able to move.
    [HideInInspector]
    public bool hasReachedDestination = false; // Used to check when 
    public void DiceRoll(InputAction.CallbackContext context) // run when player rolls dice on board
    {
        if (context.performed && hasRan == false && !stat.singlePlayer) // makes sure its only ran once
        {
            if (turnScript.RunTurn(this.gameObject, stat.turnOrder) == true) //check to see if conditions are met on TurnManager
            {
                //Set cam to follow player
                StartCoroutine(cam.ModifyCamera(this.transform, cameraSpeed, inZoom, outZoom));

                //Start update system
                cam.setCamUpdate(true);
                StartCoroutine(Moving()); // begin moving player

                
            }
        }
    }

    IEnumerator Moving() // coroutine to move around the board.
    {
        if(hasRan) { yield break; }
        hasRan = true; // prevent player from running coroutine again
        var diceRoll = Random.Range(1, 7); // pick a number from one to six

        var movesRemaining = diceRoll;

        //UI update
        turnScript.uiManager.diceSprite.gameObject.GetComponent<Animation>().Play();
        turnScript.uiManager.UpdateDiceUI(movesRemaining);

        // turning on footstep sounds
        movement.walking = true;
        while (movesRemaining > 0) // keep moving player to next tile until no more moves
        {
            if (movesRemaining > 0) // check statement so program doesn't die.
            {

                turnScript.uiManager.UpdateDiceUI(movesRemaining); // update ui for end-user

                newTile = moveManage.CallTile(stat.position, 1, stat.turnOrder); // moving one tile at a time

                stat.position++; // moving position ahead

                movesRemaining--; // decrease movement till they are out of moves left.
                while (Vector2.Distance(this.transform.position, newTile.transform.position) > 0.5)
                {
                    transform.position = Vector2.MoveTowards(this.transform.position, newTile.position, stat.speed * Time.deltaTime); // move to new position using DeltaTime
                    yield return new WaitForEndOfFrame();
                }
                newTile = null;
                //yield return new WaitForSeconds(wait); // give time to move to position.
            }
        }


        turnScript.uiManager.UpdateDiceUI(movesRemaining);
        yield return new WaitForSeconds(2);
        // turning off footstep sounds
        movement.walking = false;

        turnScript.RoundCheck(); // advance turn, see if new turn is in order.
                                     //newTile = null; // to prevent the player from moving towards the tile in the middle of the game
        hasRan = true;

        // add the buff to the players
        stat.AddBuff(moveManage.CollectBuff(stat.position));

        // show it in the UI
        turnScript.uiManager.ValueUpdate();

    }

    public void PauseGame(InputAction.CallbackContext ctx)
    {
        if(ctx.performed)
        {
            var pause = pauseMenu.GetComponent<PauseMenu>();

            pause.PauseGame();
        }
    }
}

