using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SokobotHole : MonoBehaviour
{
    public Sokobot minigame;
    public bool filled; // to prevent multiple crates from being put in the same hole
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Minigame Element") && !filled)
        {

            if (collision.GetComponent<SokobotCrate>().slotted == false)
            {
                filled = true; // prevent more crates from going in

                collision.GetComponent<SokobotCrate>().Slot(this.gameObject);
                minigame.ScoreHole(collision.GetComponent<SokobotCrate>().player);
            }
        }
    }
}

