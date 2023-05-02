using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SokobotCrate : MonoBehaviour
{
    public Rigidbody2D rb;
    public GameObject player; // player who pushes the box into the hole
    public Sokobot minigame;
    public bool slotted; // check to see if object has been slotted
    private void Start() // set rigid body in script
    {
        rb = GetComponent<Rigidbody2D>();
        this.gameObject.tag = "Minigame Element";
    }
    public void Slot(GameObject _slot) // make it unable to move from spot once completeted
    {
        slotted = true;
        rb.bodyType = RigidbodyType2D.Static; // to prevent further movement

        this.gameObject.transform.position = _slot.transform.position; // slot into the hole.
        this.gameObject.GetComponent<SpriteRenderer>().color = Color.green; // change the color to green

        minigame.holesLeft--; // decrease holes left

    }
    public void OnCollisionStay2D(Collision2D collision) // makes player the crate-pusher
    {
        if(collision.gameObject.CompareTag("Player") && collision.gameObject != player) // prevent multiple runs of redundant code
        {
            player = collision.gameObject;
        }
    }
    public void OnCollisionExit2D(Collision2D collision) // makes player not responsible for pushing the box into the hole
    {
        if(collision.gameObject == player)
        {
            player = null;
        }
        rb.velocity = Vector2.zero; // stop it sliding around the map
    }
}
