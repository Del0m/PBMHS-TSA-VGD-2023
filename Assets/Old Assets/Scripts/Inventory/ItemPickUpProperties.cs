using UnityEngine;

public class ItemPickUpProperties : MonoBehaviour
{
    /// <summary>
    /// This Script is treated as a inheritance script and not to be used as a main one
    /// </summary>

    public float radius = 3f;

    [HideInInspector]
    public GameObject player;
    //throwing isShopItem here for cleaner shopmanager code
    public bool isShopItem = false;
    public bool hasInteracted { get; private set; }

    public virtual void InteractAction()
    {
        //Do something like sub or add health
        // This method is meant to be overwritten
        // if(hasInteracted == true)
        // {
        //     Debug.Log("Interacted with " + transform.name);
        // }

        // adding on buyable piece here

    }

    private void Update()
    {
        if (player != null)
        {
            float distance = Vector3.Distance(player.transform.position, transform.position);
            if (distance < radius)
            {
                hasInteracted = true;
            }
            else if(distance > radius)
            {            
                hasInteracted = false;
            }
            InteractAction();
        }
        else
        {
            FindPlayer();
        }
    }

    public void FindPlayer()
    {
        if (PlayerManagerOld.instance.playerInstance != null)
        {
            player = PlayerManagerOld.instance.playerInstance;
        }
        else
        {
            if (PlayerManagerOld.instance == null)
            {
                Debug.LogError(PlayerManagerOld.instance.name + " Is either not found or its missing!");
                return;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
