using UnityEngine;

public class EnemyItemDrop : MonoBehaviour
{
    [Header("Set what items the enemy will drop")]
    public int RandomItemRarity;// Here will set how rare an item will spawn in. Make sure the value is negative
    public Item[] ItemPrefabs; //Here set the maximum number of items or gameobjects the enemy will drop

    private int randomItem;
    private GameObject itemPickUpObject_Template;

    private void Awake()
    {
        //Resource.load to load template that are use for dropping items
        itemPickUpObject_Template = Resources.Load("Item&Equipment/PlayerItems/InputPickUpItems/ItemPickUp_Template") as GameObject;
    }

    private void Start()
    {
        //Insert Resource load of itempickup template
        Debug.Log(itemPickUpObject_Template);

        randomItem = Random.Range(-RandomItemRarity, ItemPrefabs.Length);
    }

    public void DropRandomItem()
    {
        if (randomItem < 0)
        {
            Debug.Log("No Item has been spawned in!!");
        }
        else
        {
            if (randomItem >= 0 && itemPickUpObject_Template != null)
            {
                GameObject itemDrop = Instantiate(itemPickUpObject_Template, transform.position, Quaternion.identity);
                if(itemDrop != null)
                {
                    ItemPickup pickUp = itemDrop.gameObject.GetComponent<ItemPickup>();
                    if(pickUp != null)
                    {
                        pickUp.item = ItemPrefabs[randomItem];
                        Debug.Log("Spawned a " + ItemPrefabs[randomItem].name);
                    }
                }
            }
            else if(itemPickUpObject_Template == null)
            {
                Debug.LogError("No ItemPickUp_Template found!");
                return;
            }
        }

    }
}
