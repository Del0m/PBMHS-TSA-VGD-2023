using System.Collections;
using UnityEngine;
[RequireComponent(typeof(BossStats))]
public class EnemyBossAI : EnemyAI
{
    /*
     * This script is used for referece and as a derivitive, where ever this script is being used 
     * as a deritive it will include functions from EnemyAI as well the required component that is BossStats
     * and the headers
    */

    //[] Make a aerial phase (details on Trello)

    [Header("Boss Parameters")]
    //variables for phase changes
    public float fireRate;
    public float damage = 5f;
    public float midHeight = 1f; //Used to check if the player is at a certain height so the enemy can aim up
    [Header("Boss Attack Point")]
    public Transform AttackTransformParent;
    private bool AttackIsReady;
    
    public BossStats stats { get; private set; }

    private float nextFire = 0f;
    private Transform AttackPoint;

    public bool isTheMinotaurBoss;

    private void Awake()
    {
        stats = gameObject.GetComponent<BossStats>();
        AttackPoint = AttackTransformParent.GetChild(0).transform;

        animator = this.gameObject.GetComponent<Animator>();
    }

    public virtual void AimAtTarget()
    {
        //Meant to be overwritten 
        //Make sure to call TrackTarget() to aim track at the player
        if(player == null)
        {
            return;
        }
    }

    //When activated the enemy will track aim at the player
    public void TrackTarget(bool look)
    {
        if (look == true && player != null)
        {
            AttackTransformParent.right = (player.transform.position - AttackTransformParent.position);
        }
    }

    //Attack Phases
    public void ShootingPhase(bool IsAttacking, GameObject projectilePrefab)
    {
        AttackIsReady = IsAttacking;

        // Shooter type to shoot projectiles
        if (IsAttacking != false)
        {
            StartCoroutine(prepareToAttack(nextFire));

            if (AttackIsReady == true && Time.time > nextFire)
            {
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
            }
        }
    }

    public void MeleePhase(bool IsAttacking, GameObject areaAttackPrefab)
    {
        //melee phase
        //summoning area attack to hit player.
        if (IsAttacking != false)
        {
            StartCoroutine(prepareToAttack(nextFire));

            if (AttackIsReady == true && Time.time > nextFire)
            {
                nextFire = Time.time + fireRate;

                //Spawns in a area attack
                GameObject areaAttack = Instantiate(areaAttackPrefab, AttackPoint.transform.position, Quaternion.identity);
                
                var area = areaAttack.GetComponent<DamageArea>();
                if (area != null)
                {
                    area.areaDamage = damage;
                    //Debug.Log("Area Attack");
                }
                AttackIsReady = false;
            }
        }
    }

    IEnumerator prepareToAttack(float attackDuration)
    {  
        try
        {
            if (isTheMinotaurBoss == true)
            {
                
                yield return new WaitForSeconds(.55f);
                //animator.SetBool("isAttacking", true);
                yield return new WaitForSeconds(attackDuration -.85f); 
                AttackIsReady = true;
                //animator.SetBool("isAttacking", false);
                StopCoroutine(prepareToAttack(0f));
                
                

            }
            else 
            {
                animator.SetBool("isAttacking", true);
                yield return new WaitForSeconds(attackDuration);

                AttackIsReady = true;
                animator.SetBool("isAttacking", false);
                StopCoroutine(prepareToAttack(0f));
            }
            
        }
        finally
        {
            //do nothing

        }     
    }
    
}