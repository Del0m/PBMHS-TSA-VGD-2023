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

    //stats for player movement
    [Header("Minigame Stats")]
    public int speed = 15;
    public int jumpPower = 20;
    public int damage = 5;
    //cool down for player actions
    public double cooldown = 0.5;

   
    

    public void UpdateStat(double stat, int multiplier) // upgrades or downgrades stats permanently
    {
        // upgrade the stat by the multiplier 
        stat *= multiplier;
    }
    public IEnumerator BuffStat(double stat, int time, int multiplier) // temporarily upgrades or downgrades stats
    {
        // changing the stat to the upgraded amount
        var oldStat = stat; // variable to switch stat back to its original amount
        stat *= multiplier;

        // wait time to bring back to normal
        yield return new WaitForSeconds(time);

        // return stats back to normal
        stat = oldStat;
    }
}
