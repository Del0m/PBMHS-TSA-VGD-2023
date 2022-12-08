using UnityEngine;
using UnityEngine.UI;

public class BossStats : EntityStats
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
        if (healthCounter != null)
        {
            healthCounter.text = currentHealth + "/" + MaxHealth;
        }
    }

    public override void Die()
    {
        //coinage on death
        if (canDropCoins == true)
        {
            //grabs player instance, this being player, and gets a random range of coinage
            int coinsOnDeath = Random.Range(1, maxCoinsOnDeath);
            if (PlayerManagerOld.instance.playerInstance != null)
            {
                PlayerManagerOld.instance.playerInstance.GetComponent<PlayerStats>().CurrencyAdd(coinsOnDeath);
            }
        }
        /*
        GameManager_Temporary manager = GameManager_Temporary.instance.GetComponent<GameManager_Temporary>();
        if (manager != null)
        {
            manager.EndOfGame();
        }
        else
        {
            Debug.LogError(manager.name + " not found!");
            return;
        }
        */
        // Destroys the object this scripts sits in
        Destroy(gameObject);

        // TODO: Change this to drop something like a special item
        if(itemDrop != null)
        {
            itemDrop.DropRandomItem();
        }

        // Kill Boss
        base.Die();
        Debug.Log("Enemy boss has died!");
    }
}
