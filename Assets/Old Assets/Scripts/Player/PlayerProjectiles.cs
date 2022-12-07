using UnityEngine;

// this scripts purpose is to detect what direction the user is facing in order to tell where to shoot the item
public class PlayerProjectiles : MonoBehaviour
{
    //initalizing variables
    [HideInInspector]
    public float PlayerDamage = 10f;

    public float projSpeed = 5f;

    private Rigidbody2D rb; 

    // Start is called before the first frame update
    void Awake()
    {
        // detecting whether or not to shoot right or left
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * projSpeed;
        Destroy(this.gameObject, 4f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(this.gameObject);
        if (collision.gameObject.tag == "Enemy")
        {
            EntityStats health = collision.gameObject.GetComponent<EntityStats>();
            if (health != null)
            {
                health.TakeDamage(PlayerDamage);
                Debug.Log("Hit! Took " + PlayerDamage + "damage!");
                Destroy(this.gameObject);
            }
        }

    }

}
