using UnityEngine;
using System.Collections;

public class HydraAI : EnemyBossAI
{
    //This enemy will not have any kind of movement
    //This enemy is stationary

    [Header("hydra Parameters")]
    public GameObject areaAttackPrefab;

    private bool toFallBack = false;

    public override void CheckDistance()
    {
        base.CheckDistance();
        Vector2 self = new Vector2(transform.position.x, 0);
        Vector2 other = new Vector2(player.transform.position.x, 0);

        float pos = Vector2.Distance(self, other);

        if (pos <= maxDistance)
        {
            if (pos > minDistance)
            {
                //Keep moving
                if (!toFallBack)
                {
                    move = true;
                }
                commenceAttack = false;
            }
            else
            {
                //Attack once and fall back 

                move = false;
                transform.Translate(Vector3.zero);

                StartCoroutine(fallBack());

                commenceAttack = true;

                if (toFallBack && pos < maxDistance)
                {
                    commenceAttack = false;
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
                    StartCoroutine(pushForward());
                }
            }
        }
        else if (pos > maxDistance)
        {
            //Stop when player is out of range
            commenceAttack = false;
            move = false;
            transform.Translate(Vector3.zero);
        }

        Attack();
    }

    IEnumerator fallBack()
    {
        yield return new WaitForSeconds(3f);
        //Enable bool to fall back
        toFallBack = true;
        StopCoroutine(fallBack());
    }
    
    IEnumerator pushForward()
    {
        yield return new WaitForSeconds(5f);
        toFallBack = false;
        StopCoroutine(pushForward());
    }

    public override void AimAtTarget()
    {
        base.AimAtTarget();

        TrackTarget(true); //Always tracking the player

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

    // Update is called once per frame
    void Update()
    {
        if(player != null && player.activeSelf)
        {
            FaceTarget();
            AimAtTarget();
        }
    }

    void Phases(bool IsAttacking)
    {
        if(stats != null)
        {
            MeleePhase(IsAttacking, areaAttackPrefab);
        }
    }
}