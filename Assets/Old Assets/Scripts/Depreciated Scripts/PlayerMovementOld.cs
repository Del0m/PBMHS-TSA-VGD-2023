using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementOld : MonoBehaviour
{
    // defining the variables
    private float playerSpeed;
    private Rigidbody2D rb;

    //controls
    public float leftRight;
    public float upDown;

    //movement clamp bools
    public bool isMovingX = false;
    public bool isMovingY = false;

    // Start is called before the first frame update
    void Start()
    {
        //initalizing controls and rigidbody component for use in code
        rb = GetComponent<Rigidbody2D>();
        playerSpeed = 3.0f;
    }

    // Update is called once per frame
    void Update() 
    {
    
        // having the leftright/updown controls be like this so they can constantly update
        leftRight = Input.GetAxis("Horizontal");
        upDown = Input.GetAxis("Vertical");
        //controls for wasd movement
        XController();
        YController();
        //controls for turning
        CursorAlign();
       
    }
    //making the specific x controls
    private void YController()
    {
       //if it is moving, clamp max speed, allow y movement
        if (upDown != 0)
        {
            rb.AddForce(Time.deltaTime * upDown * playerSpeed * transform.up);
            rb.velocity = new Vector2(rb.velocity.x,Mathf.Clamp(rb.velocity.y, -playerSpeed, playerSpeed));
            isMovingY = true;
        }
        // if moving, clamp movement to 0
        else
        {
            isMovingY = false;
        }
        if (isMovingY != true)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0.00000f);
        }
        
    }
    private void XController()
    {
        //if it is moving, clamp max speed, allow X movement
        if (leftRight != 0)
        {
            rb.AddForce(Time.deltaTime * leftRight * playerSpeed * transform.right);
            rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -playerSpeed, playerSpeed), rb.velocity.y);
            isMovingX = true;
        }
        else
        //kill velocity
        {
            isMovingX = false;
        }
        if (isMovingX != true)
        {
            rb.velocity = new Vector2(0.00000f, rb.velocity.y);
        }

    }
    private void CursorAlign()
    {
        // this makes it to where the cursor defines the user's sight direction
        Vector3 mousePosition = Input.mousePosition;
        // mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        // minuses directions to where you don't have an epileptic seizure
        Vector2 direction = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);
        transform.up = direction;
    }
}
