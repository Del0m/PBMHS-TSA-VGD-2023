// armin delmo; 1/15/23
// the purpose of this program is to compile all the player stats into one script for easy use
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStats : MonoBehaviour
{
    private PlayerMovement movement;
    // board stats
    [Header("Board Stats")]
    public int position;
    public int turnOrder;

    //stats for player movement
    [Header("Minigame Stats")]
    public float speed = 15;
    public float jumpPower = 20;
    public int damage = 5;

    [Header("Multipliers")]
    public float speedMulti = 1; // to be modified by buffs whenever the buffs are activated
    public float jumpMulti = 1;
    //cool down for player actions
    public double cooldown = 0.5;

    public int wins;

    [Header("Minigame Points")]
    public bool lost;


    [Header("Player Modes")]
    public bool singlePlayer;

    [Header("Inventory")]
    public List<BuffObject> buff;

    private void Start()
    {
        movement = GetComponent<PlayerMovement>(); // find component on body
    }
    public void UpdateStat(double stat, int multiplier) // upgrades or downgrades stats permanently
    {
        // upgrade the stat by the multiplier 
        stat *= multiplier;
    }
    private bool isBuffed; // prevent player from buffing twice
    public IEnumerator BuffStat(float time, float multi) // temporarily upgrades or downgrades stats
    {
        if(isBuffed) { yield return null; } // prevent player from buffing
        isBuffed = true; // prevent multiple runs

        // adding multiplier
        speedMulti += multi;
        jumpMulti += (.45f  * multi); // less jump to prevent player from flying
        // wait time to bring back to normal
        yield return new WaitForSeconds(time);

        // return stats back to normal
        speedMulti -= multi;
        jumpMulti -= (.45f * multi);

        isBuffed = false; // allow runs again

    }
    public void ParticleBuff(BuffObject buff) // show particles on player to represent buff
    {
        movement.SetParticle(movement.buffParticle, (int)buff.time, new Quaternion(0, 0, 0, 0), buff.color, true);
    }
    public void AddBuff(BuffObject buffInc) // add buff to the player's inventory
    {
        if(!buffInc) // check if no buff is being given
        {
            return;
        }
        if(buff.Count == 2) // replace first buff with second, put new buff in second slot
        {
            buff[1] = buff[0];
            buff[1] = buffInc;
            return;
        }
        buff.Add(buffInc);
        return;
    }
    public void BuffButton(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && !isBuffed && movement.rb.bodyType == RigidbodyType2D.Dynamic) // check if player pressed button
        {
            try
            {
                var buffUsed = (int)ctx.ReadValue<float>(); // find what buff they are trying to use
                UseBuff(buff[buffUsed], buffUsed); // give buff to player
            }
            catch (Exception) { } // do nothing, no buffs were found in player's buff inventory
        }
    }
    void UseBuff(BuffObject buffType, int buffUsed) // activates the buff on the player
    {
        Debug.Log("running useBuff");
        if(!isBuffed) 
        {
            buff.RemoveAt(buffUsed); // get rid of buff
            
            ParticleBuff(buffType); // show particles on player for particle
            StartCoroutine(BuffStat(buffType.time, buffType.multiplier)); // coroutine that buffs player
        }
    }
}
