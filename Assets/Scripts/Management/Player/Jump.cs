//armindelmo Jump.cs; the purpose of this program is to reset the CanJump boolean on PlayerMovement
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Jump : MonoBehaviour
{
    //define player script
    private PlayerMovement player;

    private void Start()
    {
        player = GetComponentInParent<PlayerMovement>(); // define player to change canjump boolean
    }

    private void OnTriggerEnter2D(Collider2D collision) //enable jumping
    {
        if(collision.tag == "Ground")
        {
            player.canJump = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision) //disable jumping
    {
        if(collision.tag == "Ground")
        {
            player.canJump = false;
        }
    }
}
