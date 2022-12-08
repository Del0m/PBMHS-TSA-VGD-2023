using System.Collections;
using UnityEngine;

public class EnemyAIShooter : EnemyAI
{ 
    [Header("Attack Variables")]
    public float fireRate = 1f; 
    public float damage = 10f;
    public float midHeight = 1f; //Used to check if the player is at a certain height so the enemy can aim up

    [Header("Attack Properties")]
    public GameObject projectilePrefab;
    public Transform AttackTransformParent;

    private float nextFire = 0f;
    private Transform AttackPoint;
    private bool AttackIsReady = false;

    public Sound attackSound;

    void PlaySound()
    {
        if(attackSound == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        attackSound.source.Play();
    }

    private void Start()
    {
        if(attackSound != null)
        {
            attackSound.source = gameObject.AddComponent<AudioSource>();
            attackSound.source.clip = attackSound.clip;


            attackSound.source.volume = attackSound.volume;
            attackSound.source.pitch = attackSound.pitch;
            attackSound.source.loop = attackSound.loop;
        }
        else
        {
            Debug.LogError("No Sound effect found on " + this.name);
            return;
        }

        originalDamage = damage;
        AttackPoint = AttackTransformParent.GetChild(0).transform;
        animator = this.GetComponent<Animator>();
        
    }

    public override void Attack()
    {
        base.Attack();

        if (commenceAttack != false && player != null)
        {
            AttackPlayer(commenceAttack);
        }
        else if (commenceAttack == false && player != null)
        {
            AttackPlayer(commenceAttack);
        }
    }

    public override void CheckDistance()
    {
        base.CheckDistance();
        Vector2 self = new Vector2(transform.position.x, 0);
        Vector2 other = new Vector2(player.transform.position.x, 0);

        float pos = Vector2.Distance(self, other);

        //Specifies to check the height between entities
        float selfHeight = transform.position.y + maxHeight;
        float otherHeight = player.transform.position.y;
        // When the player is on the same height or below the enemy

        if (pos <= maxDistance && selfHeight >= otherHeight && player != null)
        {
            FaceTarget();
            if (Shooter && !Melee)
            {
                // Attack from distance
                commenceAttack = true;
                PlaySound();
                Attack();
            }
            if (pos >= minDistance)
            {
                //keep moving
                move = true;
            }
            else if (pos <= minDistance)
            {
                //Stop moving
                move = false;
                transform.Translate(Vector3.zero);
            }
        }
    }

    bool lookUp = false;

    private void LateUpdate()
    {
        RageChecker();

    }

    void Update()
    {
        //print(animator.GetBool("isAttacking"));
        if (player != null)
        {
            //The height of the player
            float otherHeight = player.transform.position.y;
            //The height that adds in this position added with a height amount
            float centerHeight = transform.position.y + midHeight;

            TrackTarget(lookUp);

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
        if (move == true && animationPause == false)
        {
            animator.SetFloat("speed", 1);
        }
        else
        {
            animator.SetFloat("speed", 0);
        }
        //animator.SetBool("isAttacking", !commenceAttack);
        //Debug.Log(animator.GetBool("isAttacking"));
        //Debug.Log(animator.GetFloat("speed"));
    }

    void TrackTarget(bool look)
    {
        if (look == true && player != null)
        {
            AttackTransformParent.right = (player.transform.position - AttackTransformParent.position);
        }
    }

    private bool animationPause;

    void AttackPlayer(bool IsAttacking)
    {
        AttackIsReady = IsAttacking;

        // Shooter type to shoot projectiles
        if (IsAttacking != false)
        {
            StartCoroutine(prepareToAttack(nextFire));

            if (AttackIsReady == true && Time.time > nextFire)
            {
                StartCoroutine(playAnimations());
                //Not do the walk animation
                // Pauses the enemy movement

                if (MovementPausing == true)
                {
                    CompleteStop = true;
                    StartCoroutine(PauseTemporary());
                }

                nextFire = Time.time + fireRate;

                // Spawns a projectile
                GameObject projectileObject = Instantiate(projectilePrefab, AttackPoint.position, Quaternion.identity);
                var projectile = projectileObject.GetComponent<EnemyProjectile>();
                Vector3 shootDir = -(AttackTransformParent.position - AttackPoint.position).normalized;
                if (projectile != null)
                {
                    
                    projectile.setup(shootDir);
                    //Gives the projectile damage
                    projectile.projectileDamage = damage;
                    //Debug.Log("Added damage to projectile!");
                }
                AttackIsReady = false;
                /*
                if(AttackIsReady != false && CompleteStop == true)
                {
                    Debug.Log("attacking!");
                    animator.SetBool("isAttacking", true);

                }
                */
            }
        }
    }
    void RageChecker()
    {
        if (!Enraged && !rage)
        {
            damage = originalDamage;
            movementSpeed = originalMoveSpeed;
        }
    }
    public override void RageModifierVoid()
    {
        base.RageModifierVoid();
        if (rage)
        {
            damage *= rageModifier;
            movementSpeed *= rageModifier;
            fireRate -= 1f;
        }
        //[] There can be another function to set how long the Rage ability lasts
    }


    IEnumerator prepareToAttack(float attackDuration)
    {
        yield return new WaitForSeconds(attackDuration);    
        AttackIsReady = true;
        
        StopCoroutine(prepareToAttack(0f));
    }

    IEnumerator playAnimations()
    {
        animationPause = true;
        //print("Doing Animation");
        animator.SetBool("isAttacking", true);
        
        yield return new WaitForSeconds(0.895f);
        animator.SetBool("isAttacking", false);
        animationPause = false;
        //print("Not doing jack shit");
        StopCoroutine(playAnimations());
    }



}
