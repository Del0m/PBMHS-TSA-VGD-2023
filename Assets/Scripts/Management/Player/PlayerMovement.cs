//armin delmo PlayerMovement.cs
//the purpose of this program is to allow xy movement for the player during minigames.
using System.Collections;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

//requiring components to allow player to move properly
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(AudioSource))]
public class PlayerMovement : MonoBehaviour
{
    private Settings settings;

    private Vector2 movement;

    public bool canJump = false;
    public bool canEverJump = false;

    public bool fallThrough = false;

    bool canDash = true;

    //things needed to move player
    public Rigidbody2D rb;

    private Controls controls;

    //player action variables
    private bool canAct = false;
    private bool canYMovement = false;
    public bool acting;
    private bool canMoveFreely = true;


    //attacking variables
    public bool isAttacking;


    //player stats to reference
    public PlayerStats stat;

    public GameObject holding; // for minigames to see if they're holding something.
    private bool canPick; // to tell if something is in the pick up radius

    [Header("Sounds")]
    public AudioClip[] footstep;
    public AudioClip dropSound;
    public AudioClip dashSound;

    [Header("Particles")]
    public GameObject particlePrefab;
    ParticleSystem particle;
    [Header("Animation")]
    public Animator animate;
    public GameObject spriteChild; // child for sprite; animation


    private void Awake()
    {
        controls = new Controls();
    }
    private void Start()
    {
        animate = this.GetComponentInChildren<Animator>(); // grab animator from player
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

        //seeing if the player is doing singleplayer
        if(stat.singlePlayer)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
        }
    }
    private void FixedUpdate()
    {
        ActCooldown(stat.cooldown);
        DashCooldown(stat.cooldown * 2);

        // animationt stuff
        AnimationUpdate();

        Movement();

        // if statement to keep held object in hand
        if (holding != null)
        {
            holding.transform.position = this.gameObject.transform.position;
        }
        StartCoroutine(Footstep());
    }


    public void AnimationUpdate() // update animations for player
    {
        animate.SetBool("isAttacking", isAttacking);
        //animate.SetInteger("Movement", (int));

        if(rb.velocity.x < 0)
        {
            spriteChild.transform.localScale = new Vector3(-Mathf.Abs(spriteChild.transform.localScale.x), spriteChild.transform.localScale.y, 1);
            spriteChild.transform.localPosition = new Vector2(-0.25f, 0);
        }
        else if (rb.velocity.x > 0)
        {
            spriteChild.transform.localScale = new Vector3(Mathf.Abs(spriteChild.transform.localScale.x), spriteChild.transform.localScale.y, 1);
            spriteChild.transform.localPosition = new Vector2(0.25f, 0);

        }
    }

    public void setFreeMovement(bool enable){
        canMoveFreely = enable;
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
            yield return null;
        }
        if(holdOngoing == false && newObject.GetComponent<HoldableItem>().canPickUp == true) // see if coroutine is already running / see if object can be picked
        {
            holdOngoing = true;
            if (isDrop == false)
            {
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
    int gScale = 3;
    public void GameSwitch(bool enable) // basic overload for games that don't need topdown or grabbing
    {
        canAct = enable;
        canJump = enable;
        if(stat.singlePlayer) { canEverJump = true; }
        else { canEverJump = enable; }
        if(enable)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
        }
        else
        {
            rb.bodyType = RigidbodyType2D.Static;
        }

        // check single player
        if(stat.singlePlayer)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.gravityScale = gScale;

        }
    }
    public void GameSwitch(bool enable, bool topDown) // overload for topdown games
    {
        GameSwitch(enable); // running base statement
        if (topDown)
        {
            rb.gravityScale = 0;
        }
        else
        {
            rb.gravityScale = gScale;
        }
        canEverJump = !topDown;
        canYMovement = topDown;
        
    }
    public void GameSwitch(bool enable, bool topDown, bool pick) // overloadstart game for the player using switch.
    {
        GameSwitch(enable, topDown); // run base statement 
        canPick = pick;
    
    }
    public void ReadMovement(InputAction.CallbackContext ctx) // to read move values of the player to use them for minigame purposes
    {
        if(ctx.performed)
        {
            if(canYMovement)
            {
                movement = new Vector2(
                ctx.ReadValue<Vector2>().x,
                ctx.ReadValue<Vector2>().y 
                );
            }
            else
            {
                movement = new Vector2(
                ctx.ReadValue<Vector2>().x,
                rb.velocity.y
                );
            }
        }
    }
    private void Movement() // clamp the movement and allow gravity to work freely.
    {
        Vector2 currVelocity = rb.velocity; // get current velocity of player

        // get movement direction
        movement = transform.TransformDirection(movement);

        Vector2 velocityChange = (movement * stat.speed - currVelocity);

        Vector2.ClampMagnitude(velocityChange, 1);
        if(!canYMovement)
        {
            velocityChange = new Vector2(velocityChange.x, 0);
        }
        else
        {
            velocityChange = new Vector2(velocityChange.x, velocityChange.y);
        }

        rb.AddForce(velocityChange, ForceMode2D.Impulse);
    }
    public void Dash(InputAction.CallbackContext ctx)
    {
        if(ctx.performed && canDash == true)
        {
            StartCoroutine(DashRoutine());
            canDash = false;
        }
    }
    public IEnumerator DashRoutine()
    {
        playInstance = dashSound;

        //Check if the sound obj exists
        playSound.PlayOneShot(playInstance, (settings.soundVolume * settings.masterVolume)); //fix this

        //if(canJump == true) { SetParticle(SetDirection(), false); } // if they're on the floor

        var originalSpeed = stat.speed;
        stat.speed *= 3;
        yield return new WaitForSeconds(0.25f);
        stat.speed = originalSpeed;
    }

    //timer for Cooldown
    private double timerAct;
    private void ActCooldown(double downtime) // prevent player from acting several times a second
    {
        if(canAct == false)
        {
            //run timer
            timerAct += Time.deltaTime;
            if(timerAct > downtime) // when timer exceeds the cooldown
            {
                //allow acting to happen again.
                timerAct = 0;
                canAct = true;
            }
        }
    }
    private double timerDash;
    private void DashCooldown(double downtime)
    {
        if(canDash == false)
        {
            timerDash += Time.deltaTime;
            if(timerDash > downtime)
            {
                timerDash = 0;
                canDash = true;
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

            canAct = false; // set cooldown
        }
    }
    public void Jump(InputAction.CallbackContext context) // input action to increase the players velocity up
    {
        if(context.performed && canJump == true && canEverJump == true) // ensures its only ran once
        {
            var jumpVelocity = new Vector2(rb.velocity.x, rb.velocity.y + stat.jumpPower);
            StartCoroutine(JumpRoutine());
            rb.AddForce(jumpVelocity, ForceMode2D.Impulse);
            // animating particles here
            SetParticle(new Quaternion(0, 0, -90, 0), false);
            particle.transform.rotation = Quaternion.Euler(new Vector3(0,0,-90));

            canJump = false; // turn off jumping to prevent them from jumping again.
        }
    }
    public IEnumerator JumpRoutine() // simple routine for the jump animnation
    {
        animate.SetBool("Jumping", true);
        yield return new WaitForSeconds(.5f);
        animate.SetBool("Jumping", false);
    }
    [Header("Audio")]
    AudioClip playInstance;
    AudioSource playSound;
    bool isPlaying;
    IEnumerator Footstep() // play footstep sound effects while moving
    {
        if(rb.velocity.x != 0 && isPlaying == false && canJump == true) // to check if x movement
        {
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
            yield return new WaitForSeconds(playInstance.length + 0.12f); // prevent overlapping sounds

            isPlaying = false; // allow sounds to play again
        }
    }
    public void DropSound()
    {
        playInstance = dropSound;
        playSound.PlayOneShot(playInstance, (settings.soundVolume * settings.masterVolume));
    }
    // use trig for this.
   /* public Quaternion SetDirection() // set direction for particle
    {
        //Vector2.Angle(this.gameObject, );
        var direction = 0;

        switch(yMovementInput)
        {
            case 1:
                direction = -90;
                break;
            case -1:
                direction = 90;
                break;
            default:
                break;
        }
        switch(xMovementInput)
        {
            case 1:
                if(direction != 0)
                {
                    switch (direction)
                    {
                        case 90:
                            direction = -45;
                            break;
                        case -90:
                            direction = 45;
                            break;
                    }
                }
                else
                {
                    direction = -180;
                }
                break;
            case -1:
                if (direction != 0)
                {
                    switch (direction)
                    {
                        case 90:
                            direction = 45;
                            break;
                        case -90:
                            direction = -45;
                            break;
                    }
                }
                else
                {
                    direction = 0;
                }
                break;
            default:
                break;
        }
        return new Quaternion(0, 0, direction, 0);
    }
   */
    void SetParticle(Quaternion rot, bool hasParent)
    {
        var particleObject = Instantiate(particlePrefab, this.gameObject.transform.position, rot);

        if (hasParent) { particleObject.transform.SetParent(this.transform); } // give parent, usually only neededd for jump

        particleObject.transform.rotation = rot;
        particle = particleObject.GetComponent<ParticleSystem>();

        particle.gameObject.transform.rotation = rot;

    }
    void SetParticle(Quaternion rot, Vector2 pos, bool hasParent) // play particle in specific position
    {
        var particleObject = Instantiate(particlePrefab, this.gameObject.transform.position, this.transform.rotation);

        if(hasParent) { particleObject.transform.SetParent(this.transform); } // give parent, usually only neededd for jump
        particle = particleObject.GetComponent<ParticleSystem>();

        particle.gameObject.transform.position = pos;
        particle.gameObject.transform.rotation = rot;

    }

}
