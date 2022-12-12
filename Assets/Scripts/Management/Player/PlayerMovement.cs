//armin delmo PlayerMovement.cs
//the purpose of this program is to allow xy movement for the player during minigames.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//requiring components to allow player to move properly
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    //stats for player movement
    public int speed = 2;
    public float movementInput;

    private bool canJump;
    //things needed to move player
    private Rigidbody2D rb;

    private Controls controls;

    private void Awake()
    {
        //initalize controls class
        controls = new Controls();
    }
    private void Start()
    {
        // initalizing player controls and gameplay input
        controls.Gameplay.Move.Enable();

        this.gameObject.tag = "Player"; //set player tag to "Player"
        rb = this.GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        XMovement();
    }
    private void XMovement() // for the purposes of moving the player left and right
    {
        //running player movement in here.
        movementInput = controls.Gameplay.Move.ReadValue<Vector2>().x; // read x value of movement input
        rb.velocity = new Vector2((movementInput * speed), rb.velocity.y); // change velocity to move player, but don't change y velocity.
    }
    public void Jump(InputAction.CallbackContext context) // input action to increase the players velocity up
    {
        if(context.performed && canJump == true) // ensures its only ran once
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y + 10);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("collision detected");
        if (collision.collider.tag == "Ground")
        {
            canJump = true;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        Debug.Log("collision gone");
        if (collision.collider.tag == "Ground")
        {
            canJump = false;
        }
    }
}
