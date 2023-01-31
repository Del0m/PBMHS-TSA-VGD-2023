// armin delmo; 1/15/23
// the purpose of this program is to compile all the player stats into one script for easy use
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    // board stats
    [Header("Board Stats")]
    public int position;
    public int turnOrder;

    //stats for player movement
    [Header("Minigame Stats")]
    public float speed = 15;
    public float jumpPower = 20;
    public int damage = 5;
    //cool down for player actions
    public double cooldown = 0.5;

    public int wins;

    [Header("Minigame Points")]
    public bool lost;


    [Header("Player Modes")]
    public bool singlePlayer;
   
    public void UpdateStat(double stat, int multiplier) // upgrades or downgrades stats permanently
    {
        // upgrade the stat by the multiplier 
        stat *= multiplier;
    }
    public IEnumerator BuffStat(double stat, double time, double multiplier) // temporarily upgrades or downgrades stats
    {
        // changing the stat to the upgraded amount
        var oldStat = stat; // variable to switch stat back to its original amount
        stat *= multiplier;
        Debug.Log("Old Stat:" + oldStat + "New Stat: " + stat);

        // wait time to bring back to normal
        yield return new WaitForSeconds(((float)time));

        // return stats back to normal
        stat = oldStat;
    }
}
