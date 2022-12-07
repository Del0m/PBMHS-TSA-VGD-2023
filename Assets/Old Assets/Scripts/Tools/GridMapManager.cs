using UnityEngine;

public class GridMapManager : MonoBehaviour
{
    /// <summary>
    /// This script detects the player that can be accesed by enemies that are
    /// colliding it, to make the enemy that there is a player in their area
    /// </summary>

    public bool PlayerIsInArea { get; private set; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            PlayerIsInArea = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            PlayerIsInArea = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            PlayerIsInArea = false;
        }
    }
}
