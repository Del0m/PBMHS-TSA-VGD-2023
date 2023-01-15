// armin delmo; 1/15/23
// the purpose of this program is to manage the entity stats for the object.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityStats : MonoBehaviour
{
    [Header("Stats")]
    public int health;
    public int speed;
    public float iFrame; // to prevent several hits from the same object.
    //public int jumpPower; // I don't know if this will be needed, ever...

    private bool invulnerable;
    public IEnumerator TakeDamage(int damage) // damage coroutine to be ran everytime a player damages this object
    {
        if(invulnerable == false)
        {
            invulnerable = true;
            health -= damage;

            yield return new WaitForSeconds(iFrame);
            invulnerable = false;
        }


    }
    
}
