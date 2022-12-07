using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    //initalizing variables necessary for combat

    public float shotCooldown;

    public GameObject projectile;
    public GameObject player;
    public Sound harpNote;

    public Transform attackPoint;
    [HideInInspector]
    public float damage = 10f;

    public bool readyToFire { get; private set; } = true;  

    // Start is called before the first frame update
    void Awake()
    {
        // will be necessary for actually being able to fire lyre
        harpNote.source = gameObject.AddComponent<AudioSource>();
        harpNote.source.clip = harpNote.clip;

        harpNote.source.volume = harpNote.volume;
        harpNote.source.pitch = harpNote.pitch;
        harpNote.source.loop = harpNote.loop;
    }

    void Play()
    {
        if (harpNote == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        harpNote.source.Play();
    }

    // Update is called once per frame
    void Update()
    {
        // putting in voids so they get called
        FiringModule();
    }

    void FiringModule()
    {
        // control so it actually detects if it shoots
        if (Input.GetKeyDown(KeyCode.Mouse0) && readyToFire == true)
        {
            readyToFire = false;
            //this is for player position detection
            GameObject projectileObject = Instantiate(projectile, attackPoint.position, player.transform.rotation);
            Play();
            var projectileValues = projectileObject.GetComponent<PlayerProjectiles>();
            if (projectileValues != null)
            {
                projectileValues.PlayerDamage = damage;
            }
            readyToFire = false;
            StartCoroutine(resetFire());
        }

    }

   IEnumerator resetFire()
    {
        yield return new WaitForSeconds(shotCooldown);
        readyToFire = true;
        StopCoroutine(resetFire());
    }
}