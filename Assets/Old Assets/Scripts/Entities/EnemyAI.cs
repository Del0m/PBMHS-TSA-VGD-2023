using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class EnemyAI : MonoBehaviour
{
    [Header("Movement Parameters")]
    public float movementSpeed = 6f;

    [Header("Distance Parameters")]
    public float minDistance = 2f;
    public float maxDistance = 10f;

    [Header("Height Parameters")]
    public float maxHeight = 1.5f;

    [HideInInspector]
    public bool move = false;
    [HideInInspector]
    public bool commenceAttack;  // 'Get' means to use as reference and 'private set' means it can be changed only inside this script
    //values for the purpose of returning to normal values after rage
    [HideInInspector]
    public float originalMoveSpeed; 
    [HideInInspector]
    public float originalDamage;
    [HideInInspector]
    //time to rage is kept as an int for the purpose of using system threading sleep
    public float timeToRage;

    [Header("Enemy Class")]
    public bool Melee;
    public bool Shooter;
    [Header("Enemy Abilities & Behaviors")]
    public bool Enraged;
    public bool MovementPausing; 

    [HideInInspector]
    public float rageModifier = 1.3f;

    [HideInInspector]
    public float maxTimeToRage = 20f;

    public bool rage { get; private set; } = false;
 
    [HideInInspector]
    public bool CompleteStop = false;

    //following for animation

    [HideInInspector]
    public Animator animator;

    private new SpriteRenderer renderer;
    //Used temporarily for testing and will be changed on Final release

    public virtual void OnValidate()
    {
        if (Melee && Shooter)
        {
            Melee = false;
            Shooter = false;
            Debug.LogError("Enemy set to more than one class, make sure it's set one class!");
            return;
        }
        else if (!Melee && !Shooter)
        {
            Debug.LogError("Enemy is not set to a class");
        }
    }

    public Rigidbody2D rb { get; private set; }

    public GameObject player { get; private set; }

    void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        renderer = gameObject.GetComponent<SpriteRenderer>();
        originalMoveSpeed = movementSpeed;
        animator = this.GetComponent<Animator>();
        timeToRage = Random.Range(5f, maxTimeToRage);
        animator = this.GetComponent<Animator>();
        StartCoroutine(ForceGridUpdate());
    }

    IEnumerator ForceGridUpdate()
    {
        yield return new WaitForSeconds(5f);
        rb.AddForce(Vector2.up * 2f);
        checkIsStuck = false;
        StopCoroutine(ForceGridUpdate());
    }

    void CheckForPhysics()
    {
        if (rb == null)
        {
            rb = gameObject.GetComponent<Rigidbody2D>();
            Debug.LogWarning(this.name + " is not getting the rigidbody correctly");
            return;
        }
    }

    bool playerCheck = true;

    void FixedUpdate()
    {
        CheckForPhysics();
        CheckForStuck();
        if (rage)
        {
            visualRageIndicator();
        }
        if (playerCheck)
        {
            if (player != null && player.activeSelf)
            {
                //Checks for the players distance from the enemy
                CheckDistance();
                //Moves the Entity
                Movement();
            }
            else //Note the boolean 'playerInGridMap' maybe 
            {
                CheckForTarget();
            }
        }
    }

    void CheckForTarget()
    {
        //Checks for the playerManager and sets the player as the target for the enemy
        if (PlayerManagerOld.instance.playerInstance != null)
        {
            player = PlayerManagerOld.instance.playerInstance;
            print("Player found!");
        }
        else
        {
            // If there isn't a PlayerManager it will give an error
            Debug.LogError(PlayerManagerOld.instance.name + " Is either not found or its missing!");
            playerCheck = false;
            return;
        }
    }

    public virtual void CheckDistance()
    {
        // Meant to be overwritten

        // This code will be universal for all derivitives
        // Specify what the Enemy does depending on the player's distance
        // Specifies to check the distance on the x-axis
        Vector2 self = new Vector2(transform.position.x, 0);
        Vector2 other = new Vector2(player.transform.position.x, 0);

        float pos = Vector2.Distance(self, other);

        //Specifies to check the height between entities
        float selfHeight = transform.position.y + maxHeight;
        float otherHeight = player.transform.position.y;

        if(getEnraged(pos) == true)
        {
            Debug.Log("Calling Rage");
        }

        // When the player is on the same height or below the enemy
        if (pos >= maxDistance || selfHeight <= otherHeight) //When player is on top or above the enemy
        {
            move = false; // Stops moving when player is outside the max distance
            commenceAttack = false;
            //transform.Translate(Vector2.zero);
        }else if (player.activeSelf == false || player == null)
        {
            move = false; // When player is set to false the enemy will stop 
            commenceAttack = false;
            //transform.Translate(Vector2.zero);
        }
    }

    bool getEnraged(float pos)
    {
        if (Enraged && pos < maxDistance)
        {
            Invoke("Rage", timeToRage);
            if (rage)
            {
                Enraged = false;
                return true;
            }
        }
        return false;
    }


    [Header("To Invert Facement")]
    public bool isInverted = false;

    [HideInInspector]
    public bool faceRight = true;

    public void FaceTarget()
    {
        if(player != null && isInverted == false)
        {
            if(transform.position.x < player.transform.position.x && !faceRight)
            {
                // Debug.Log("Player is in front of the enemy");
                //transform.localScale = new Vector3(1f, 1f, 1f);
                faceRight = !faceRight;

                transform.Rotate(0f, 180f, 0f);

            }else if(transform.position.x > player.transform.position.x && faceRight)
            {
                //Debug.Log("PLayer is in behind the enemy");
                //transform.localScale = new Vector3(-1f, 1f, 1f);
                faceRight = !faceRight;

                transform.Rotate(0f, 180f, 0f);
            }
        }else if(player != null && isInverted != false)
        {
            if (transform.position.x < player.transform.position.x && faceRight)
            {
                // Debug.Log("Player is in front of the enemy");
                //transform.localScale = new Vector3(1f, 1f, 1f);
                faceRight = !faceRight;

                transform.Rotate(0f, 180f, 0f);

            }
            else if (transform.position.x > player.transform.position.x && !faceRight)
            {
                //Debug.Log("PLayer is in behind the enemy");
                //transform.localScale = new Vector3(-1f, 1f, 1f);
                faceRight = !faceRight;

                transform.Rotate(0f, 180f, 0f);
            }
        }
    }

    public virtual void Movement()
    {
        if (move == true && CompleteStop == false && checkIsStuck == false)
        {
            //Movement
           
            if (transform.position.x < player.transform.position.x)
            {
                //move right
                rb.velocity = new Vector2(movementSpeed, rb.velocity.y);
            }
            else if (transform.position.x > player.transform.position.x)
            {
                //move left
                rb.velocity = new Vector2(-movementSpeed, rb.velocity.y);
            }

            //animation
            /*try
            {
                animator.SetFloat("speed", Mathf.Abs(rb.velocity.x));
                Debug.Log("Changing speed!");

            }
            finally
            {
                //goes to nothing, no variable found

            }
            */

        }
    }

    public virtual void Attack()
    {
        // Shoot or do melee attacks
        // This is meant to be overwritten
        // if (player.activeSelf)
        //  {
        //      Debug.Log("Attacking " + player.name);
        // }
    }
    public virtual void RageModifierVoid()
    {
        //mod rage...
        Debug.Log("Modifying the enemy!");
    }
    public void visualRageIndicator()
    {
        if (rage)
        {
            if(renderer != null)
            {
                renderer.color = new Color(1, 0.72f, 0.72f, 1);
            }
            else
            {
                Debug.LogError("No renderer found on " + this.name);
                return;
            }
        }
    }

    private bool checkIsStuck = true;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Environment") && checkIsStuck == true)
        {
            Debug.LogWarning(this.name + " was stuck inside a environment object");
            Destroy(this.gameObject);
        }
    }

    void CheckForStuck()
    {
        if (checkIsStuck == true && rb != null)
        {
            rb.constraints = RigidbodyConstraints2D.FreezePosition;
            //print("Checking if enemy is stuck");
            StartCoroutine(waitForColliders());
        }
        else if(checkIsStuck == false && rb != null)
        {
            rb.constraints = RigidbodyConstraints2D.None;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }

    IEnumerator waitForColliders()
    {
        yield return new WaitForSeconds(1f);
        checkIsStuck = false;
        rb.constraints = RigidbodyConstraints2D.None;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        StopCoroutine(waitForColliders());
    }

    void Rage()
    {
        if (Enraged)
        {
            Debug.Log(this.name + " is enraged!");
            rage = true;
            RageModifierVoid();
        }
    }

    public IEnumerator PauseTemporary()
    {
        //Stops and continues after a second
        //Debug.Log(this.name + " paused!");
        yield return new WaitForSeconds(0.5f);
        CompleteStop = false;
        StopCoroutine(PauseTemporary());
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(254f, 218f, 0, 0.2f);
        Gizmos.DrawWireSphere(transform.position, maxDistance);
    }
}