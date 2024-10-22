// armin delmo; 1/15/23
// the purpose of this program is to manage the entity stats for the object.
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class EntityStats : MonoBehaviour
{
    [Header("Stats")]
    public float health;

    public int speed;
    public float iFrame; // to prevent several hits from the same object.
    //public int jumpPower; // I don't know if this will be needed, ever...

    private bool invulnerable;

    public bool isDead;
    [HideInInspector]
    public GameObject killer;

    [Header("Sounds")]
    public AudioClip hit;
    public AudioSource soundPlayer;
    public Settings settings;


    public GameObject objectToKill; // for the purposes of some objects being children
    [HideInInspector]
    public float totalHealth; // just for reading

    [Header("Special Stuff")]
    public GameObject healthBar; // for the purposes of updating the health
    public bool bossBar;
    public PlayerUIManager uiManager;
    public virtual void Start()
    {
        totalHealth = health; // initalizing max health

        if(soundPlayer == null)
        {
            this.GetComponent<AudioSource>(); // to prevent audio error
        }
        if (settings == null)
        {
            settings = GameObject.FindGameObjectWithTag("Settings").GetComponent<Settings>();
        }
        if(uiManager == null)
        {
            uiManager = GameObject.FindGameObjectWithTag("PlayerUIManager").GetComponent<PlayerUIManager>(); // grabbing the player manager to mod event bar
        }

    }
    IEnumerator DamageFlash() // indicate damage has been dealt to object. Also plays sounds!
    {
        var objectColor = this.GetComponent<SpriteRenderer>();
        soundPlayer.PlayOneShot(hit, (settings.masterVolume * settings.soundVolume));
        for(int i = 0; i < objectColor.color.r; i+=5)
        {
            objectColor.color = new Color(objectColor.color.r + i,0,0); // progressively turn more red
            yield return new WaitForSeconds(0.1f); // to prevent from changing color too fast
        }
        objectColor.color = Color.white;
    }
    public virtual IEnumerator TakeDamage(int damage, GameObject obj) // damage coroutine to be ran everytime a player damages this object
    {
        if(invulnerable == false)
        {
            obj.GetComponent<PlayerMovement>().isAttacking = false; // to prevent player attacking again
            invulnerable = true;
            health -= damage;

            // flash red to indicate damage has been dealt
            StartCoroutine(DamageFlash());

            DeathCheck(obj);

            yield return new WaitForSeconds(iFrame);
            invulnerable = false;
        }


    }
    public virtual void DeathCheck(GameObject kill) // check to see if entity is dead
    {
        if(health > 0) 
        {
            Debug.Log("Returning...");
            return; 
        } // stop running if player is above 0 hp.

        isDead = true;
        killer = kill;
        Destroy(objectToKill, 1f);

    }
    // triggers to find player
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            var plrStat = collision.GetComponent<PlayerStats>();
            if (collision.GetComponent<PlayerMovement>().isAttacking == true) // plr atk?
            {
                StartCoroutine(TakeDamage(plrStat.damage, collision.gameObject));
            }
        }
    }

}
