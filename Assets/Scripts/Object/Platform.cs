using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    private void OnCollisionStay2D(Collision2D collision)
    {
        //checking if player and if they want to fall through
        if (collision.collider.tag == "Player" && collision.collider.GetComponent<PlayerMovement>().fallThrough == true)
        {
            //disable collision, enable trigger
            Physics2D.IgnoreCollision(this.gameObject.GetComponent<Collider2D>(), collision.collider, false);
            this.gameObject.GetComponent<Collider2D>().isTrigger = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        //checking if player and if they want to fall through
        if (collision.tag == "Player" && collision.GetComponent<PlayerMovement>().fallThrough == false)
        {
            //enable collision, disable trigger
            Physics2D.IgnoreCollision(this.gameObject.GetComponent<Collider2D>(), collision, false);
            this.gameObject.GetComponent<Collider2D>().isTrigger = false;
        }

    }
}
