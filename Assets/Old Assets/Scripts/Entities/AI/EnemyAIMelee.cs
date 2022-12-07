using UnityEngine;
using System.Collections;

public class EnemyAIMelee : EnemyAI
{
    [Header("Attack Variables")]
    public float fireRate = 1f;
    public float damage = 10f;
    public float AttackRange = 1.5f;

    private float nextFire = 0f;
    private bool AttackIsReady = false;

    // private EnemyAI enemy;

    // void start for the purpose of assigning rage values
    private void Start()
    {
        // assigning og values
        originalDamage = damage;
    }

    public override void Attack()
    {
        base.Attack();

        if (commenceAttack != false && player != null)
        {
            AttackPlayer(commenceAttack);
        } else if (commenceAttack == false && player != null)
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
       // float selfHeight = transform.position.y + maxHeight;
        //float otherHeight = player.transform.position.y;
        // When the player is on the same height or below the enemy

        if (pos <= maxDistance)
        {
            FaceTarget();
            if (pos >= minDistance)
            {
                //keep moving and don't attack
                move = true;
            }
            else
            {
                //Stop moving and start attacking
                if (!Shooter && Melee)
                {
                    commenceAttack = true;
                    Attack();
                }
                move = false;
                transform.Translate(Vector3.zero);
            }
        }
    }

    private void Update()
    {
        if (move == true)
        {
            animator.SetFloat("speed", 1);
        }
        else
        {
            animator.SetFloat("speed", 0);
        }
    }

    void LateUpdate()
    {
        RageChecker();

        animator.SetBool("isAttacking", AttackIsReady);
        //Debug.Log(animator.GetBool("isAttacking"));
    }
    void AttackPlayer(bool IsAttacking)
    {
        AttackIsReady = IsAttacking;

        if (Vector3.Distance(transform.position, player.transform.position) < AttackRange)
        {
            if (IsAttacking != false)
            {
                StartCoroutine(prepareToAttack(fireRate));

                if (AttackIsReady == true && Time.time > nextFire)
                {
                    // Pauses the enemy movement

                    if (MovementPausing == true)
                    {
                        CompleteStop = true;
                        StartCoroutine(PauseTemporary());
                    }

                    nextFire = Time.time + fireRate;

                    // Within attack range
                    EntityStats health = player.GetComponent<EntityStats>();
                    if(health != null)
                    {
                        // Deal damage to the player
                        health.TakeDamage(damage);
                        Debug.Log(player.name + " took " + damage + " damage");

                        AttackIsReady = false;
                    }
                }
            }
        }
        else
        {
            //Resets the attacking cycle
            StopAllCoroutines();
            AttackIsReady = false;
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
    }

    IEnumerator prepareToAttack(float attackDuration)
    {
        

        yield return new WaitForSeconds(attackDuration);
        Debug.Log("stopping animation");
        
        
        AttackIsReady = true;
        StopAllCoroutines();
    }
}