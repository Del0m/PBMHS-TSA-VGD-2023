// armin delmo; PlayerAnimation.cs; this will control the player's animations.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public Animator anim; // player animation that will be controlled
    public PlayerMovement player; // for the purpose of checking movement dependent variables for the character.

    //varaibles that will define animations for the animator
    public int movement;
    public bool jumping;
    public bool attacking;
    // collected from parent
    public Rigidbody2D rb;

    public int CheckSpeed()
    {
        var dir = 0;
        if(player.canEverJump)
        {
             dir = (int)(rb.velocity.x);
        }
        else
        {
            dir = (int)(rb.velocity.x + rb.velocity.y);
        }   
        return dir;
    }
    public void OrientationUpdate() // updates the orientation of the player while in the game
    {
        movement = CheckSpeed(); // find the current direction the player is movement in.

        if(movement < 0)
        {
            this.gameObject.transform.localScale = new Vector3(-Mathf.Abs(this.gameObject.transform.localScale.x), this.gameObject.transform.localScale.y, 1);
            this.gameObject.transform.localPosition = new Vector2(-0.25f, 0);
        }
        else if(movement > 0)
        {
            this.gameObject.transform.localScale = new Vector3(Mathf.Abs(this.gameObject.transform.localScale.x), this.gameObject.transform.localScale.y, 1);
            this.gameObject.transform.localPosition = new Vector2(0.25f, 0);
        }
        else
        {
            // do nothing :)
        }
    }
    public void CheckMovement() // combines "Check Orientation, CheckSpeed" and checks the players movement animation.
    {
        OrientationUpdate();
        // formula for setting the speed number to one
        anim.SetInteger("Movement", movement);

        // check if the player is moving
        if(movement != 0)
        {
            player.walking = true;
        }
        else
        {
            player.walking = false;
        }
    }
    public IEnumerator CheckAttack() // simple routine for the attack animation
    {
        anim.SetBool("isAttacking", true);
        yield return new WaitForSeconds(.25f);
        anim.SetBool("isAttacking", false);
    }
    public Quaternion ParticleDirection() // calculates what direction the particles should be in
    {
        var vect = rb.velocity;
        var angle = Vector2.Angle(new Vector2(0, 0), vect);

        angle = -angle;
        angle += 360;

        return new Quaternion(0,0,angle,0);
    }
}
