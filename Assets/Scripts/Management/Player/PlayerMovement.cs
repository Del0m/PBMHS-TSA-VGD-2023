//armin delmo PlayerMovement.cs
//the purpose of this program is to allow xy movement for the player during minigames.
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.InputSystem;

//requiring components to allow player to move properly
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    public InputDevice currentDevice; // for game handler to grab and put on new instantiations

    //stats for player movement
    [Header("Stats")]
    public int speed = 2;
    public int jumpPower = 10;
    
    private float movementInput;
    public bool canJump = false;

    public bool fallThrough = false;

    //things needed to move player
    private Rigidbody2D rb;

    private Controls controls;

    //player action variables
    private bool canAct = false;
    public bool acting;
    //cool down for player actions
    private double _cooldown = 0.5; // base cooldown for player

    private void Awake()
    {
        controls = new Controls();
    }
    private void Start()
    {
        // initalizing player controls and gameplay input
        controls.Gameplay.Move.Enable();
        //initalize controls class
        

        this.gameObject.tag = "Player"; //set player tag to "Player"
        rb = this.GetComponent<Rigidbody2D>();

        //setting inputdevice to player for game manager
    }
    private void FixedUpdate()
    {
        XMovement();
        Cooldown(_cooldown);
    }
    public void GameSwitch(bool enable) // start game for the player using switch.
    {
        switch(enable)
        {
            case true:
                canAct = true;
                canJump = true;
                rb.bodyType = RigidbodyType2D.Dynamic;
                break;
            case false:
                canAct = false;
                canJump = false;
                rb.bodyType = RigidbodyType2D.Static;
                break;
        }
    }
    private void XMovement() // for the purposes of moving the player left and right
    {
        //running player movement in here.
        movementInput = controls.Gameplay.Move.ReadValue<Vector2>().x; // read x value of movement input
        rb.velocity = new Vector2((movementInput * speed), rb.velocity.y); // change velocity to move player, but don't change y velocity.
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
    IEnumerator ActRoutine()
    {
        acting = true;
        yield return new WaitForSeconds(0.2f);
        acting = false;
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
        if(context.performed && canJump == true) // ensures its only ran once
        {
            Debug.Log("Jumping!");
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y + jumpPower);
            canJump = false; // turn off jumping to prevent them from jumping again.
        }
    }
}
