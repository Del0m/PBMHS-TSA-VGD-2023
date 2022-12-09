/*
 *
 * Purpose of the script is to be used dynamically in mutilple minigames
 * that need a 2D player controls and movement with a already built 
 *
 * @author Yahir Bonilla
 * @version December 8, 2022
 */

using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerInput))]
public class MiniGamePlayerControls : MonoBehaviour
{
    [Header("Player Parameters")]
    public float speed = 5.0f;
    public Controls controls;
    public PlayerInput playerInput;

    //define private vars
    private Rigidbody2D rb;
    private float velocity;
    private Vector2 direction;


    // Start is called before the first frame update
    void Awake()
    {
        //Get component from the gameObject this script is sitting on
        rb = gameObject.GetComponent<Rigidbody2D>();
        playerInput = this.gameObject.GetComponent<PlayerInput>();

        //initialize controls
        controls = new Controls();
    }
    
    public void Controls(InputAction.CallbackContext context)
    {
        //Set a Vector2 based upon what button direction is 
        direction = context.ReadValue<Vector2>();
    }
    
    public void xControls(InputAction.CallbackContext context)
    {
        //Set a Vector2 based upon what button direction is
        direction.x = context.ReadValue<Vector2>().x;
    }

    public void yControls(InputAction.CallbackContext context)
    {
        //Set a Vector2 based upon what button direction is
        direction.y = context.ReadValue<Vector2>().y;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Check if there is input
        if((direction.x > 0 || direction.x < 0)|| (direction.y > 0 || direction.y < 0)) {
            //Move based upon the direction
            rb.AddForce(speed * direction, ForceMode2D.Impulse);
            rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -speed, speed), rb.velocity.y);
        }
        else
        {
            //Stop rigidbody velocity
            rb.velocity = new Vector2(0.0f, rb.velocity.y);
        }
    }
}
