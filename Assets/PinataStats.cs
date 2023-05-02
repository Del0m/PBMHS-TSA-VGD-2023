// pinatastats.cs; this is a derivative of the "EntityStats" script so it can be geared towards the pinata
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinataStats : EntityStats
{
    public Healthbar healthScript;
    public PinataObject pinata;

    public override void Start()
    {
        base.Start();
        pinata = GetComponent<PinataObject>();
        healthScript = GameObject.FindGameObjectWithTag("Health Bar").GetComponent<Healthbar>();
    }
    public override IEnumerator TakeDamage(int damage, GameObject obj)
    {
        yield return base.TakeDamage(damage, obj);
        healthScript.ModifyProgress(health, totalHealth);
    }
    public override void DeathCheck(GameObject kill)
    {
        base.DeathCheck(kill);
        if(health <= 0)
        {
            pinata.FindKiller(kill);
        }
    }
}
