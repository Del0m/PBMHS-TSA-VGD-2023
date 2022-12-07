using UnityEngine;

public class BansheeAI : EnemyBossAI
{
    //For Aerial this enemy will act like a turret
    //Once aerial is disabled the enemy will act like a moving shooter but doesn't fly

    [Header("Banshee Parameters")]
    public GameObject projectilePrefab;

    private bool aerialmode;
    private float currentDmg;

    public Sound attackSound;

    void PlaySound()
    {
        if (attackSound == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        attackSound.source.Play();
    }


    private void Start()
    {
        currentDmg = damage;
    }

    // Update is called once per frame
    void Update()
    {
        if(player != null && player.activeSelf)
        {
            FaceTarget();
            AimAtTarget();
        }
        if(rb != null)
            CheckForAerial();
    }

    public override void AimAtTarget()
    {
        base.AimAtTarget();

        TrackTarget(true);
    }

    void CheckForAerial()
    {
        if (aerialmode)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        else
        {
            rb.constraints = RigidbodyConstraints2D.None;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }

    public override void Attack()
    {
        base.Attack();
        if (commenceAttack != false && player != null)
        {
            Phases(commenceAttack);
        }
        else if (commenceAttack == false && player != null)
        {
            Phases(commenceAttack);
        }
    }

    void Phases(bool IsAttacking)
    {
        if(stats != null)
        {
            ShootingPhase(IsAttacking, projectilePrefab);
            if(stats.currentHealth > stats.MaxHealth / 1.5f) // Initial phase
            {
                //Do shooting attacks and move in the air
                aerialmode = true;
            }
            else if(stats.currentHealth <= stats.MaxHealth / 1.5f)
            {
                //Drop down on ground, get more damage, and still do shooting attacks
                if(setParamters() == true)
                {
                    aerialmode = false;
                    //print(this.name + " is no longer flying");
                    ShootingPhase(IsAttacking, projectilePrefab);
                }
                else
                {
                    Debug.LogError(this.name + " parameter change has encountered an error, check the code or something went wrong when calling the function");
                }
            }
        }
        else
        {
            Debug.LogError("No Boss Stats Script found on " + this.name);
            return;
        }
    }

    bool setParamters()
    {
        if(damage == currentDmg)
        {
            damage *= 1.3f;
            Debug.Log(this.name + " is now in second phase");
            return true;
        }
        return false;
    }

    public override void CheckDistance()
    {
        base.CheckDistance();

        // variables for use of distance checking
        Vector2 self = new Vector2(transform.position.x, 0);
        Vector2 other = new Vector2(player.transform.position.x, 0);

        float pos = Vector2.Distance(self, other);

        if(pos <= maxDistance)
        {
            commenceAttack = true;
            Attack();
            if(pos > minDistance && aerialmode == false)
            {
                //Keep moving
                move = true;
            }
            else if(pos < minDistance && aerialmode == false)
            {
                //Stop moving
                move = false;

                //If player gets close, move backwards
                if(pos < minDistance - 2.2f)
                {
                    if (transform.position.x < player.transform.position.x)
                    {
                        //move left
                        rb.velocity = new Vector2(-movementSpeed, rb.velocity.y);
                    }
                    else if (transform.position.x > player.transform.position.x)
                    {
                        //move right
                        rb.velocity = new Vector2(movementSpeed, rb.velocity.y);
                    }
                }
            }
        }
    }
}
