using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(ParticleMaster))]
//[RequireComponent (typeof(PlayerFeet))]
[RequireComponent(typeof(Animator))]
public class PlayerControllerOld : MonoBehaviour
{
    public bool playerControllerPause = true;
    //player for easy referencing
    // defines the player speed publicly which can be changed by other scripts
    [HideInInspector]
    public float playerSpeed;   
    public float jumpPower;

    private Rigidbody2D rb;
    // public variable since it will be modded alot
    private bool isJumping;

    // controls
    private float leftRight;

    // move clamper
    private bool xMoving = false;

    // variable to tell whether or not user is left or right
    private float turnDirection = 1f;
    bool faceRight = true;
    //particles and other artsy things
    [Header("Particle/Jump Parameters")]
    public Transform playerFeet;
    private ParticleMaster master;

    private PlayerFeet feet;
    private bool playerDisabled = true;
    
    //animation vars
    public Animator animator;
    private bool isFiring;
    private float yVelocity;

    //Vine climbing parameters
    private bool isClimbing;
    //private bool isLadder;

    public Sound jumpImpact;

    private void Awake()
    {
        jumpImpact.source = gameObject.AddComponent<AudioSource>();
        jumpImpact.source.clip = jumpImpact.clip;


        jumpImpact.source.volume = jumpImpact.volume;
        jumpImpact.source.pitch = jumpImpact.pitch;
        jumpImpact.source.loop = jumpImpact.loop;
    }

    void Play()
    {
        if (jumpImpact == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        jumpImpact.source.Play();
    }

    // Start is called before the first frame update
    void Start()
    {
        // control initalize
        if (playerControllerPause)
        {
            StartCoroutine(enableThePlayer());
        }
        rb = this.GetComponent<Rigidbody2D>();
        feet = this.GetComponent<PlayerFeet>();
        master = gameObject.GetComponent<ParticleMaster>();
        isJumping = false;
        if (master != null)
        {
            if (master.entity != gameObject)
            {
                master.setEntity(gameObject);
            }
        }
        else if (master == null)
        {
            Debug.LogError("No ParticleMaster script found on " + gameObject.name);
            return;
        }
        
        if(playerControllerPause == true)
        {
            StartCoroutine(enableThePlayer());
        }
        else
        {
            playerDisabled = false;
        }

        animator = this.GetComponent<Animator>();
    }

    //This might be removed 
    IEnumerator enableThePlayer()
    {
        yield return new WaitForSeconds(1f);
        playerDisabled = false;
        StopAllCoroutines();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(playerDisabled != true)
        {
            //putting in controls so they can actively update
            leftRight = Input.GetAxis("Horizontal");
           
            //Sets the jump boolean to the player feet boolean
            isJumping = feet.canJump;
            //print(isJumping);
           
            //controls
            xControls();
            Jumping();
            MoveClamp();
            SpriteMover();
            
            //sprite mover and bullet direction detector
            SpriteProjectileMover();
            AnimationManger();

            //Depreceated
            //climbingInput();


            yVelocity = rb.velocity.y;
        }
        else
        {
            Debug.Log("Player is disabled");
        }
    }
    private void xControls()
    {
        // asking when leftright be moving so can move player
        if (leftRight != 0)
        {
            rb.AddForce(playerSpeed * transform.right, ForceMode2D.Impulse);
            rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -playerSpeed, playerSpeed), rb.velocity.y);
            xMoving = true;
            //particle doesn't work rn
            //master.InstantiateParticle("movement", "dustOnMovement", playerFeet);
        }
        else
        // killing velocity
        {
            xMoving = false;
        }
        if (xMoving != true)
        {
            rb.velocity = new Vector2(0.0f, rb.velocity.y);
        }
    }
    private void Jumping()
    {
        //this is meant for jump controls for the player. 
        if (Input.GetKey(KeyCode.Space) && isJumping == false && playerFeet != null)
        {
            //adding the force to properly jump
            rb.AddForce(Time.deltaTime * jumpPower * transform.up, ForceMode2D.Impulse);
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -jumpPower, jumpPower));
            isJumping = true;
            //particle master reference for dustOnJump effect
            if(master != null)
            {
                master.InstantiateParticle("movement", "dustOnJump", playerFeet);
            }
        }
    }


    /*
    void climbingInput()
    {
        float input = Input.GetAxis("Vertical");
        climbingPhysics(input);
        if(isLadder && Mathf.Abs(input) > 0f)
        {
            isClimbing = true;
        }
    }
    */

    void climbingPhysics(float i)
    {
        if (isClimbing)
        {
            rb.gravityScale = 0.5f;
            rb.velocity = new Vector2(rb.velocity.x, i * playerSpeed);
        }
        else
        {
            rb.gravityScale = 1.2f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //when the user hits the ground with the tag ground it will reset the jumps
        if (collision.gameObject.layer == LayerMask.NameToLayer("Environment"))
        {
            if (master != null)
            {
                Play();
                master.InstantiateParticle("movement", "dustOnJump", playerFeet);
            }
        }
        /*
        else if(collision.gameObject.tag == "Ladder")
        {
            isLadder = true;
        }
        */
    }

    /*
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Ladder")
        {
            isLadder = false;
            isClimbing = false;
        }
    }
    */
    private void MoveClamp()
    {
        // this serves the purpose of forcing the player to be clamped to these speeds in order to not exceed the speeds set in the program.
        rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -playerSpeed, playerSpeed), rb.velocity.y);

        rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -1000, 2*jumpPower));
    }

    private void SpriteProjectileMover()
    {
        //sprite mover and bullet direction detector
        if (leftRight > 0)
        {
            turnDirection = 1f;
        }
        else if (leftRight < 0)
        {
            turnDirection = -1f;
        }
    }

    private void SpriteMover()
    {
        if (turnDirection == -1f && faceRight)
        {
            faceRight = !faceRight;
            transform.Rotate(0f, 180f, 0f);

        }
        else if (turnDirection == 1f && !faceRight)
        {
            faceRight = !faceRight;
            transform.Rotate(0f, 180f, 0f);

        }
    }
    //keep all animation variables in 1place
    private void AnimationManger()
    {
        animator.SetFloat("speed", Mathf.Abs(leftRight));
        animator.SetFloat("isJumping",yVelocity);

        //firing
        isFiring = this.gameObject.GetComponent<PlayerCombat>().readyToFire;
        animator.SetBool("isAttacking", !isFiring);
    } 
}
