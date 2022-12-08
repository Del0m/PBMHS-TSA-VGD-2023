using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(ParticleMaster))]
public class EntityStats : MonoBehaviour
{
    [Header("Stats")]
    public float MaxHealth = 100f;
    [HideInInspector]
    public float currentHealth; //{ get; private set; }
    public float Armor = 0f;

    private new SpriteRenderer renderer;
    private ParticleMaster master;

    public Sound hitEffect;
    /*
    private void Awake()
    {
        if(hitEffect != null)
        {
            hitEffect.source = this.gameObject.AddComponent<AudioSource>();
            hitEffect.source.clip = hitEffect.clip;


            hitEffect.source.volume = hitEffect.volume;
            hitEffect.source.pitch = hitEffect.pitch;
            hitEffect.source.loop = hitEffect.loop;
        }
        else
        {
            Debug.LogError("No effect is mention on " + this.name);
            return;
        }
    }
    */
    private void Start()
    {
        if (hitEffect != null)
        {
            hitEffect.source = this.gameObject.AddComponent<AudioSource>();
            hitEffect.source.clip = hitEffect.clip;


            hitEffect.source.volume = hitEffect.volume;
            hitEffect.source.pitch = hitEffect.pitch;
            hitEffect.source.loop = hitEffect.loop;
        }
        else
        {
            Debug.LogError("No effect is mention on " + this.name);
            return;
        }

        renderer = gameObject.GetComponent<SpriteRenderer>();
        currentHealth = MaxHealth; // sets current health to the max health
        master = this.GetComponent<ParticleMaster>();

        if (master != null)
        {
            if (master.entity != gameObject)
            {
                master.setEntity(gameObject);
            }
        }
        else if (master == null)
        {
            Debug.LogError("No ParticleMaster script found on " + gameObject.name);
            return;
        }
    }

    void Play()
    {
        if (hitEffect == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        else
        {
            hitEffect.source.Play();
        }
    }

    public void TakeDamage(float damage) // This method is called from other scripts and passes in the damage to the entity
    {
        hitIndicator();
        damage -= Armor;
        currentHealth -= damage; // Substracts health with the given damage
        if(hitEffect != null)
        {
            Play();
        }
        if(currentHealth <= 0f) // When the health reaches zero or lower
        {
            Die(); // The entity will die in a certain way
        }
    }

    void hitIndicator()
    {
        if(currentHealth > 0f && renderer != null)
        {
            //change sprite renderer color to red

            renderer.color = Color.red;

            //Use Ienumerator to change back color
            StartCoroutine(returnDefualt());
        }
        else
        {
            Debug.LogError("SpriteRendere not found on " + this.name);
            return;
        }
    }

    IEnumerator returnDefualt()
    {
        yield return new WaitForSeconds(0.1f);
        renderer.color = Color.white;
        StopCoroutine(returnDefualt());
    }

    void FixedUpdate()
    {
        if (currentHealth > MaxHealth)
        {
            currentHealth = MaxHealth;
        }
        else if(currentHealth <= 0f)
        {
            Debug.LogWarning(this.name + " didn't update the Die() function properly and this message is to say that the function call was forced to update.");
            Die();
        }
    }

    public void RestoreHealth(float amount)
    {
        if (currentHealth < MaxHealth)
        {
            currentHealth = currentHealth + amount;
        }
    }

    virtual public void Die()
    {
        Debug.Log(gameObject.name + " has died!");
        // Make the entity dissappear 
        // Make something specific in how this entity will die

        //death particles
        master.InstantiateParticle("combat", "enemyDeath", gameObject.transform);
        Destroy(gameObject);
    }
}