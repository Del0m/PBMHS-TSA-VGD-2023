// armin delmo; 1/16/2023
// the purpose of this program is to control the pinata object that will be spawned in Pummel the Pinata
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EntityStats))] // require stats
[RequireComponent(typeof(Rigidbody2D))]

public class PinataObject : MonoBehaviour
{
    public EntityStats stat; // stat page to use triggers, etc.
    public Rigidbody2D rb; // rigid body to add bouncy material and move pinata
    public PummelPinata minigame; // minigame to reference back to

    private void Start()
    {
        if(stat == null)
        {
            stat = GetComponent<EntityStats>();
        }
        rb.velocity = ChooseVectorDirection() * 3; // set random direction for pinata.
    }
    public void FindKiller(GameObject killer)
    {
        minigame.killer = killer;
        var killerNum = killer.GetComponent<PlayerStats>().turnOrder;

        StartCoroutine(minigame.EndGame(killerNum));
    }
    Vector2 ChooseVectorDirection() // calculates random vector to shoot pinata at
    {
        var angle = Random.Range(0, 360); // random angle 0 - 360
        var radAngle = angle * Mathf.Deg2Rad; // turn to radians to calculate vector

        var randVect = new Vector2(Mathf.Cos(radAngle), Mathf.Sin(radAngle)); //some weird trig shit, dunno what it means.
        return randVect;
    }
}
