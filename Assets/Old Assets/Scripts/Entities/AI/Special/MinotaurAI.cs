using UnityEngine;

public class MinotaurAI : EnemyBossAI
{
    [Header("Minotaur Parameters")]
    public GameObject areaAttackPrefab;
    public Sound roar;
    public GameObject self;
    private void Start()
    {
        roar.source = gameObject.AddComponent<AudioSource>();
        roar.source.clip = roar.clip;
        self = this.gameObject;


        roar.source.volume = roar.volume;
        roar.source.pitch = roar.pitch;
        roar.source.loop = roar.loop;
    }

    void Play()
    {
        if(roar == null)
        {
            Debug.LogWarning("Sound: " + name + "not found!");
            return;
        }
        roar.source.Play();
    }

    void Update()
    {
        if(player != null && player.activeSelf)
        {
            FaceTarget();
            AimAtTarget();
        }
        if (move == true)
        {
            animator.SetFloat("speed", 1);
        }
        else
        {
            animator.SetFloat("speed", 0);
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

    bool lookUp = false;

    public override void AimAtTarget()
    {
        base.AimAtTarget();

        if (player != null)
        {
            //The height of the player
            float otherHeight = player.transform.position.y;
            //The height that adds in this position added with a height amount
            float centerHeight = transform.position.y + midHeight;

            TrackTarget(lookUp);

            if (!isInverted)
            {
                if (otherHeight > centerHeight && !lookUp)
                {
                    lookUp = !lookUp;
                    //print("Looking up");
                }
                else if (otherHeight < centerHeight && lookUp)
                {
                    lookUp = !lookUp;
                    print("Looking front");
                    if (transform.position.x < player.transform.position.x)
                    {
                        //Aim right
                        AttackTransformParent.localEulerAngles = new Vector3(0, transform.rotation.y, transform.rotation.x);
                    }
                    else if (transform.position.x > player.transform.position.x)
                    {
                        //Aim left
                        AttackTransformParent.localEulerAngles = new Vector3(0, -transform.rotation.y, -transform.rotation.x);
                    }
                }
            }
            else
            {
                if (otherHeight > centerHeight && !lookUp)
                {
                    lookUp = !lookUp;
                    //print("Looking up");
                }
                else if (otherHeight < centerHeight && lookUp)
                {
                    lookUp = !lookUp;
                    //print("Looking front");
                    if (transform.position.x > player.transform.position.x)
                    {
                        //Aim right
                        AttackTransformParent.localEulerAngles = new Vector3(0, transform.rotation.y, transform.rotation.x);
                    }
                    else if (transform.position.x < player.transform.position.x)
                    {
                        //Aim left
                        AttackTransformParent.localEulerAngles = new Vector3(0, -transform.rotation.y, -transform.rotation.x);
                    }
                }
            }
        }
    }

    void Phases(bool IsAttacking)
    {
        if (stats != null)
        {
            MeleePhase(IsAttacking, areaAttackPrefab);
            if (stats.currentHealth < stats.MaxHealth / 2)//Second phase
            {
                //Do a melee attack and RAGE
                if(setRAGEParameters() == true)
                {
                    Debug.Log(this.name + " is now in second phase and hes's mad");
                }
            }
        }
        else
        {
            Debug.LogError("No Boss Stats Script found on " + this.name);
            return;
        }
    }

    bool setRAGEParameters()
    {
        //Used for when changing to rage phase
        if (stats.Armor <= 0f)
        {
            stats.Armor += 10f;
            visualRageIndicator();
            damage *= 1.5f;
            movementSpeed *= 2f;
            fireRate -= 1f;
            Play();
            return true;
        }
        else
        {
            return false;
        }
    }

    public override void CheckDistance()
    {
        base.CheckDistance();

        // variables for use of distance checking
        Vector2 self = new Vector2(transform.position.x, transform.position.y);
        Vector2 other = new Vector2(player.transform.position.x, player.transform.position.y);

        float pos = Vector2.Distance(self, other);

        if (pos <= maxDistance)
        {
            if (pos > minDistance)
            {
                //keep moving
                move = true;
                commenceAttack = false;
            }
            else // Move backwards
            {
                //Stop Moving
                move = false;

                commenceAttack = true;

                //Change the movement
                if (pos < minDistance / 1.5f)
                {
                    //move backwards
                    //print("Player is near the boss!");
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
        else
        {
            commenceAttack = false;
        }

        Attack();

    }
}
