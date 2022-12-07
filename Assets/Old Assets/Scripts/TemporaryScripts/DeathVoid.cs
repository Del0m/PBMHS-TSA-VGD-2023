using UnityEngine;

public class DeathVoid : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            PlayerStats stats = collision.gameObject.GetComponent<PlayerStats>();
            if(stats != null)
            {
                stats.Die();
            }
        }
    }
}
