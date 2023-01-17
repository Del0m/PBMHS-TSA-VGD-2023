//armin delmo PlayerMovement.cs
//the purpose of this program is to allow xy movement for the player during minigames.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.InputSystem;

//requiring components to allow player to move properly
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(AudioSource))]
public class PlayerMovement : MonoBehaviour
{
    private Settings settings;

    private float xMovementInput;
    private float yMovementInput;

    public bool canJump = false;
    public bool canEverJump = false;

    public bool fallThrough = false;

    //things needed to move player
    private Rigidbody2D rb;

    private Controls controls;

    //player action variables
    private bool canAct = false;
    private bool canYMovement = false;
    public bool acting;

    //attacking variables
    public bool isAttacking;


    //player stats to reference
    public PlayerStats stat;

    public GameObject holding; // for minigames to see if they're holding something.
    private bool canPick; // to tell if something is in the pick up radius

    [Header("Sounds")]
    public AudioClip[] footstep;
    public AudioClip jumpSound;
    public AudioClip dropSound;

    private void Awake()
    {
        controls = new Controls();
    }
    private void Start()
    {
        // finding settings object
        if(settings == null)
        {
            settings = GameObject.FindGameObjectWithTag("Settings").GetComponent<Settings>();
        }

        // initalizing player controls and gameplay input
        controls.Gameplay.Move.Enable();
        //initalize controls class
        

        this.gameObject.tag = "Player"; //set player tag to "Player"
        rb = this.GetComponent<Rigidbody2D>();

        // setting audiosource for player
        playSound = this.GetComponent<AudioSource>();
    }
    private void FixedUpdate()
    {
        Cooldown(stat.cooldown);
        // for movement
        rb.velocity = new Vector2((xMovementInput * stat.speed), rb.velocity.y); // move x
        if(canYMovement)
        {
            rb.velocity = new Vector2(rb.velocity.x, (yMovementInput * stat.speed)); // move y
        }

        // if statement to keep held object in hand
        if (holding != null)
        {
            holding.transform.position = this.gameObject.transform.position;
        }
        StartCoroutine(Footstep());
    }
    private void OnTriggerEnter2D(Collider2D collision) // for the purposes of holding new objects
    {
        if(collision.tag == "Minigame Element")
        {
            if(holding == null)
            {
                // change boolean to true to allow items to be picked up
                canPick = true;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        canPick = false; // prevent pickup of new items
    }
    bool holdOngoing = false;
    public IEnumerator HoldRoutine(bool isDrop,GameObject newObject) // routine to change the new gameObject that player is holding
    {
        Debug.Log("Running!");
        if(canPick == false) // kill statement to prevent players in minigames grabbing things they shouldn't
        {
            Debug.Log("Not running...");
            yield return null;
        }
        if(holdOngoing == false && newObject.GetComponent<HoldableItem>().canPickUp == true) // see if coroutine is already running / see if object can be picked
        {
            Debug.Log("Attempting to Grab Item!");
            holdOngoing = true;
            if (isDrop == false)
            {
                Debug.Log("Grabbing Item!");
                holding = newObject; // change gameObject being held to the new one
                newObject.GetComponent<HoldableItem>().beingHeld = true; // to register for minigames
                canPick = false; // prevent pickup of new items
            }
            else
            {
                holding = null;
                newObject.GetComponent<HoldableItem>().beingHeld = false; // disable hold for minigames
            }
            yield return new WaitForSeconds(0.5f);
            holdOngoing = false;
        }
        
        yield return null;
    }
    public void GameSwitch(bool enable, bool topDown, bool pick) // start game for the player using switch.
    {
        switch(enable)
        {
            case true:
                canAct = true;
                canJump = true;
                canEverJump = true;
                rb.bodyType = RigidbodyType2D.Dynamic;
                break;
            case false:
                canAct = false;
                canJump = false;
                canEverJump = false;
                rb.bodyType = RigidbodyType2D.Static;
                break;
        }
        switch(topDown) // for games to enable yMovement
        {
            case true:
                canEverJump = false;
                canYMovement = true;

                rb.gravityScale = 0; // prevents slow-fall of player

                break;
            case false:
                canEverJump = true;
                canYMovement = false;

                rb.gravityScale = 1; // allows fall of player

                break;
                
        }
        switch(pick)
        {
            case true:
                canPick = true;
                break;
            default:
                canPick = false;
                break;
        }    
    }
    public double ReadMoveValues(char ch) // to read move values of the player to use them for minigame purposes
    {
        if(ch == 'x')
        {
            return xMovementInput;
        }
        if(ch == 'y')
        {
            return yMovementInput;
        }
        Debug.LogError("Error, please change ReadMoveValues to either 'x', or 'y' ");
        return 0; // fail statement
    }
    public void XMovement(InputAction.CallbackContext ctx) // for the purposes of moving the player left and right
    {
        //running player movement in here.

        xMovementInput = ctx.ReadValue<Vector2>().x; // read x value of movement input

    }
    public void YMovement(InputAction.CallbackContext ctx) // player moves up and down
    {
        //running player movement in here.
        yMovementInput = ctx.ReadValue<Vector2>().y; // read x value of movement input
    }

    //timer for Cooldown
    private double timer;
    private void Cooldown(double downtime) // prevent player from acting several times a second
    {
        if(canAct == false)
        {
            //run timer
            timer += Time.deltaTime;
            if(timer > downtime) // when timer exceeds the cooldown
            {
                //allow acting to happen again.
                timer = 0;
                canAct = true;
            }
        }
    }
    IEnumerator ActRoutine() // coroutine to enable - disable temp vars
    {
        acting = true;
        isAttacking = true;
        yield return new WaitForSeconds(0.2f);
        acting = false;
        isAttacking = false;
    }
    public void Drop(InputAction.CallbackContext context)
    {
        //check to see when stops, when stops; disallow moving.
        if(context.performed) { fallThrough = true; }
        if(context.canceled) { fallThrough = false; }
    }
    public void Act(InputAction.CallbackContext context) // uses cooldown to allow player to act on limited interval.
    {
        //prevent repeating actions, and cooldown
        if(context.performed && canAct == true)
        {
            //run animations

            //turn on bool for acting to allow minigames to recognize boolean
            StartCoroutine(ActRoutine()); // run coroutine to have acting on for a bit.
            Debug.Log("Acting!");
            canAct = false; // set cooldown
        }
    }
    public void Jump(InputAction.CallbackContext context) // input action to increase the players velocity up
    {
        if(context.performed && canJump == true && canEverJump == true) // ensures its only ran once
        {
            Debug.Log("Jumping!");
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y + stat.jumpPower);
            canJump = false; // turn off jumping to prevent them from jumping again.
        }
    }

    AudioClip playInstance;
    AudioSource playSound;
    bool isPlaying;
    IEnumerator Footstep() // play footstep sound effects while moving
    {
        if(rb.velocity.x != 0 && isPlaying == false && canJump == true) // to check if x movement
        {
            Debug.Log("Attempting to play!");
            isPlaying = true; // prevent multiple sounds from playing

            playInstance = footstep[UnityEngine.Random.Range(0, footstep.Length)];
            playSound.PlayOneShot(playInstance, (settings.soundVolume*settings.masterVolume));
            yield return new WaitForSeconds(playInstance.length + 0.15f); // prevent overlapping sounds

            isPlaying = false; // allow sounds to play again

        }
        if(rb.velocity.y != 0 && isPlaying == false && canYMovement == true)
        {
            isPlaying = true; // prevent multiple sounds from playing

            playInstance = footstep[UnityEngine.Random.Range(0, footstep.Length)];
            playSound.PlayOneShot(playInstance, (settings.soundVolume * settings.masterVolume));
            yield return new WaitForSeconds(playInstance.length + 0.15f); // prevent overlapping sounds

            isPlaying = false; // allow sounds to play again
        }
    }
    public void JumpDrop(bool jump)
    {
        switch (jump)
        {
            case false:

                playInstance = dropSound;
                Debug.Log(settings.soundVolume * settings.masterVolume);
                playSound.PlayOneShot(playInstance, (settings.soundVolume * settings.masterVolume));

                break;
            case true:

                playInstance = jumpSound;
                playSound.PlayOneShot(playInstance, (settings.soundVolume * settings.masterVolume));

                break;
        }
    }

}
