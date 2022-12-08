using UnityEngine;

public class HealthCalculator : MonoBehaviour
{
    // defining the variables to be able to grab players health and adjust health bar size accordingly
    private GameObject player;
    private float health;
    private Vector3 healthBarSize;

    void FindPlayer()
    {
        if (PlayerManagerOld.instance.playerInstance != null)
        {
            player = PlayerManagerOld.instance.playerInstance;
            Debug.Log(player.name + " found!");
        }
        else
        {
            // If there isn't a PlayerManager it will give an error
            Debug.LogError(PlayerManagerOld.instance.name + " Is either not found or its missing!");
            return;
        }
    }

    // Update is called once per frame
    void Start()
    {
        if (player == null)
        {
            FindPlayer();
            //Sets the healthbar to not go beyond the negative numbers
        }
    }
    void Update()
    {
        if(player != null && !resetBar)
        {
            HealthProcedure();
        }
        if (resetBar)
        {
            gameObject.transform.localScale = new Vector3(0f, 0f, 0f);
        }
    }
   
    public bool resetBar = false;

    void HealthProcedure()
    {
        //grabbing hp value
        PlayerStats stats = player.GetComponent<PlayerStats>();

        if(stats != null)
        {
            health = stats.currentHealth;
            stats.setHealthBar(this.gameObject);

            //if statement used to change health bar size
            if (health <= 100)
            {
                //changing hp value
                gameObject.transform.localScale = new Vector3(health * .01f, 0.9f, 1);
            }
            else if (health <= 0f)
            {
                gameObject.transform.localScale = new Vector3(0f, 0f, 0f);
            }
        }
    }
}
