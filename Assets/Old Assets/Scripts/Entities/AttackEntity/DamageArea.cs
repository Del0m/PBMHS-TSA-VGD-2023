using UnityEngine;
using System.Collections;
public class DamageArea : MonoBehaviour
{
    [HideInInspector]
    public float areaDamage;

    [Header("Parameters")]
    public Vector2 damageAreaSize = new Vector2(1, 1);

    private GameObject minotaur;
    private Animator animator;

    private BoxCollider2D col;


    void Awake()
    {
        col = this.GetComponent<BoxCollider2D>();
        minotaur = GameObject.Find("Minotaur_MiniBoss");
        //Checks if there is a minotaur enemy object
        if(minotaur != null)
        {
            animator = minotaur.GetComponent<MinotaurAI>().animator;
            animator.SetBool("isAttacking", true);
            if (animator != null)
            {
                //nothing
            }
            else
            {
                animator = minotaur.GetComponent<MinotaurAI>().animator;
            }
        }
        else
        {
            return;
        }
    }
    void Start()
    {
        col.size = damageAreaSize;

        if(minotaur != null)
        {
            animator.SetBool("isAttacking", true);
        }
        else
        {
            return;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.position, col.size);
    }

    //area attack, if player is in zone, it hit player, simple as that
    IEnumerator AttackAnimation()
    {
        //stop animation, kill object.
        /*print("Stopping!");
        yield return new WaitForSeconds(1f);
        animator.SetBool("isAttacking", false);
        */
        yield return new WaitForSeconds(.85f);
        if(animator != null)
        {
            animator.SetBool("isAttacking", false);
        }
        Destroy(gameObject);

    }
    private void OnTriggerEnter2D(Collider2D collide)
    {
        
        if (collide.gameObject.CompareTag("Player") && areaDamage > 0f)
        {
            
            PlayerStats player = collide.gameObject.GetComponent<PlayerStats>();
            if(player != null)
            {
                player.currentHealth -= areaDamage;
                StartCoroutine("AttackAnimation");
            }
            return;
        }
        else
        {
            if(minotaur != null)
            {
                StartCoroutine("AttackAnimation");
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

}
/*animator.SetBool("isAttacking", false);*/

