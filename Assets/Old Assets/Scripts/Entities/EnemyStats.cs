using UnityEngine.UI;
using UnityEngine;

public class EnemyStats : EntityStats
{
    public GameObject healthBarObject;
    public Text healthCounter;

    private HealthBar healthBar;

    //economy system

    // coins on death allows the actual coin count to be dropped, max is the max possible the enemy can drop.
    [HideInInspector]
    public int coinsOnDeath;
    public int maxCoinsOnDeath = 5;

    [Header("Bools")]
    public bool canDropCoins;

    /*
    //for particles
    private ParticleMaster master;
    private ParticleSystem bleedOnDeath;
    private ParticleSystem bleedOnHit;

    
    private void Awake()
    {
        healthBar = healthBarObject.GetComponent<HealthBar>();
        healthBar.SetMaxHealth(MaxHealth);
        master = gameObject.GetComponent<ParticleMaster>();
        bleedOnDeath = Resources.Load<ParticleSystem>("Particles/death");
        bleedOnHit = Resources.Load<ParticleSystem>("Particles/hurt");

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
    */

    private EnemyItemDrop itemDrop;

    private void Awake()
    {
        healthBar = healthBarObject.GetComponent<HealthBar>();
        healthBar.SetMaxHealth(MaxHealth);
        itemDrop = gameObject.GetComponent<EnemyItemDrop>();
    }

    private void LateUpdate()
    {
        healthBar.SetHealth(currentHealth);
        if(healthCounter != null)
        {
            healthCounter.text = currentHealth + "/" + MaxHealth;
        }
    }

    public override void Die()
    {
        base.Die();
       // if (master != null)
       // {
       //     print("I'm dying!");
      //      master.InstantiateParticle(bleedOnDeath, "combat", "deathOnHit", gameObject.transform);
      //  }
        //remove 1 tally from the enemiesLeft counter in the gridlocker
        if(itemDrop != null)
        {
            itemDrop.DropRandomItem();
        }
        if (this.GetComponentInParent<GridLocker>() != null)
        {
            this.GetComponentInParent<GridLocker>().enemiesLeft -= 1;
        }
        else
        {
            Debug.Log("Grid Locker not found! Not submitting enemiesLeft variable");
        }
        //coinage on death
        if (canDropCoins == true)
        {
            //grabs player instance, this being player, and gets a random range of coinage
            int coinsOnDeath = Random.Range(1, maxCoinsOnDeath);
            if(PlayerManagerOld.instance.playerInstance != null)
            {
                PlayerManagerOld.instance.playerInstance.GetComponent<PlayerStats>().CurrencyAdd(coinsOnDeath);
            }
        }
        // Destroys the object this scripts sits in
        Destroy(gameObject);
    }
}
