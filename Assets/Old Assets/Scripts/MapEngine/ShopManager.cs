// TO BE PUT IN SHOP GRIDMAP PREFAB

using System.Collections;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    //making game object variables to call all the available items in the game, along with array of spawnpoints
    public Item[] possibleShopItems;

    private int spawnSelection = 0;
    private int itemSelection = 0;
    private int totalSpawnLocations;
    private int currentSpawnLocation;

    private GameObject itemPickUpObject_Template;

    public Transform[] spawnLocations;

    //bools
    private bool stop = false;

    private void Awake()
    {
        ErrorCheck();
        totalSpawnLocations = spawnLocations.Length;
        //Resource.load to load template that are use for dropping items
        itemPickUpObject_Template = Resources.Load("Item&Equipment/PlayerItems/InputPickUpItems/ItemPickUp_Template") as GameObject;
    }

    IEnumerator shopItemPlacement()
    {    
        while (!stop && itemPickUpObject_Template != null)
        {          
            for (int i = 0; i <= spawnLocations.Length; i++)
            {
                WaitForSeconds wait = new WaitForSeconds(1f);

                //debugging for the purpose of making sure code = good
                Debug.Log(i);
                Debug.Log("Spawning an item at " + spawnLocations[i]);
                Debug.Log(spawnLocations[i]);

                //spawn in shop item and using debug to confirm, also increasing possiblespawns by 1
                GameObject itemDrop = Instantiate(itemPickUpObject_Template, spawnLocations[spawnSelection].position, Quaternion.identity);
                if (itemDrop != null)
                {
                    ItemPickup pickUp = itemDrop.gameObject.GetComponent<ItemPickup>();
                    if (pickUp != null)
                    {
                        pickUp.item = possibleShopItems[itemSelection];
                        ItemPickUpProperties item = itemDrop.gameObject.GetComponent<ItemPickUpProperties>();
                        if (item != null)
                        {
                            item.isShopItem = true;
                        }
                        else
                        {
                            Debug.LogError(possibleShopItems[itemSelection].name + " doesn't contain ItemPickUpProperties.cs!");
                            stop = true;
                        }
                    }
                }
                spawnSelection++;
                Debug.Log(possibleShopItems[itemSelection]);

                // for purpose of stopping for statement
                if (spawnSelection == spawnLocations.Length)
                {
                    stop = true;
                }
                yield return wait;
            }
        }
    }
    void ErrorCheck()
    {
        if(spawnLocations.Length < 0 || possibleShopItems.Length < 0)
        {
            Debug.LogError("No spawn locations or shop items detected! Did you forget to bind them?");
            stop = true;
            return;
        }
    }

    void CheckTotalItems()
    {
        //Checks if all items have been spawned on all needed spawn points
        currentSpawnLocation = spawnSelection;
        if(currentSpawnLocation == totalSpawnLocations)
        {
            stop = true;
            StopAllCoroutines();
        }
    }

    // Update is called once per frame
    void Update()
    {
        itemSelection = Random.Range(0, possibleShopItems.Length);
        CheckTotalItems();
        StartCoroutine(shopItemPlacement());
    }

}