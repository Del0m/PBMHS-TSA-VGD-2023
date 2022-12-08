using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory instance;

    private GameObject itemDropLocation; 

    public delegate void OnItemChanged();

    public OnItemChanged onItemChangedCallBack;

    private Text coinCounter;

    public int itemSpace = 2;
    public int equipmentSpace = 2;

    public List<Item> items;
    public List<Equipment> equipments;

    GameObject player;
    private PlayerStats stats;

    //calling on resource.load as its easier and we don't need to go through the tedious task of assigning it on the script on placement. shown later in the code.
    private GameObject itemPickUpObject_Template;

    private GameObject equipmentPickUpObject_Template;
    
    #region Singleton
    private void Awake()
    {
        itemDropLocation = GameObject.FindGameObjectWithTag("PlayerTransform");
        if(instance != null)
        {
            Debug.LogWarning("Multiple instances of player inventory found!");
        }
        instance = this;
        //resource.load to load templates that are used for player enhancements (items and equipment)
        equipmentPickUpObject_Template = Resources.Load("Item&Equipment/PlayerItems/InputPickUpEquipment/EquipmentPickUp_Template") as GameObject;
        itemPickUpObject_Template = Resources.Load("Item&Equipment/PlayerItems/InputPickUpItems/ItemPickUp_Template") as GameObject;
    
    }

    
    #endregion

    void Start()
    {
        //loading resources
        
        Debug.Log(equipmentPickUpObject_Template);
        Debug.Log(itemPickUpObject_Template);

        items = new List<Item>(itemSpace);
        equipments = new List<Equipment>(equipmentSpace);
        for (int i = 0; i < itemSpace; i++)
            items.Add(null);
        for (int i = 0; i < equipmentSpace; i++)
            equipments.Add(null);

        coinCounter = GameObject.FindGameObjectWithTag("CoinCounter").GetComponent<Text>();
    }

    public bool AddItem(Item item)
    {
        if (!item.isImmediatelyConsumable)
        {
            int slot = (int)item.itemType;
            if (items[slot] != null)
            {
                //Drop the item physically by adding it to a item object and will spawn on the attack point of the player
                DropItemAndReplace(slot, item);

                print("Replaced item slot " + slot);

                if (onItemChangedCallBack != null)
                    onItemChangedCallBack.Invoke();

                return false;
            }else if(items[slot] == null)
            {
                items[slot] = item;
            }
            
            if (onItemChangedCallBack != null)
                onItemChangedCallBack.Invoke();
        }

        return true;
    }

    void DropItemAndReplace(int slot, Item item)
    {
        if(itemPickUpObject_Template != null)
        {
            GameObject obj = Instantiate(itemPickUpObject_Template, itemDropLocation.transform.position, Quaternion.identity);
            if (obj != null)
            {
                ItemPickup pickup = obj.gameObject.GetComponent<ItemPickup>();
                pickup.enabled = true;
                if (pickup != null)
                {
                    pickup.item = items[slot];
                    pickup.addItemToRender(); //Updates the script to add the item to the sprite renderer
                    print("Dropped " + items[slot].name);

                    //Replace item in the slot, its placed in here just so to let the script make the old item into a object
                    items[slot] = item;
                }
                else if(pickup == null || pickup.enabled == false)
                {
                    Debug.LogError(obj.name + " doesn't have the script named ItemPickup.cs or script is not being activated!");
                    return;
                }
            }
            else
            {
                Debug.LogError("There has been an issue with instantiating the itempickup_template");
                return;
            }
        }
    }

    void DropEquipmentAndReplace(int slot, Equipment equipment)
    {
        if (equipmentPickUpObject_Template != null)
        {
            GameObject obj = Instantiate(equipmentPickUpObject_Template, itemDropLocation.transform.position, Quaternion.identity);
            if (obj != null)
            {
                EquipmentPickUp pickup = obj.gameObject.GetComponent<EquipmentPickUp>();
                if (pickup != null)
                {
                    pickup.equipment = equipments[slot];
                    pickup.addEquipmentToRender(); //Updates the script to add the item to the sprite renderer
                    print("Dropped " + equipments[slot].name);

                    //Replace item in the slot, its placed in here just so to let the script make the old item into a object
                    equipments[slot] = equipment;
                }
                else
                {
                    Debug.LogError(obj.name + " doesn't have the script named ItemPickup.cs!");
                    return;
                }
            }
            else
            {
                Debug.LogError("There has been an issue with instantiating the itempickup_template");
                return;
            }
        }
    }

    public bool AddEquipment(Equipment equipment)
    {
        if (!equipment.isDefaultItem)
        {
            int slot = (int)equipment.equipementClass;
            if (equipments[slot] != null)
            {
                UnEquip(equipments[slot]);

                //Drops equipment physically and replaces it with another
                DropEquipmentAndReplace(slot, equipment);

                //Adds the new added equipments properties
                OnEquip(equipments[slot]);

                print("Replaced equipment slot " + slot);

                if (onItemChangedCallBack != null)
                    onItemChangedCallBack.Invoke();

                return false;
            }
            else if(equipments[slot] == null) 
            {
                equipments[slot] = equipment;
                OnEquip(equipment);
            }

            if (onItemChangedCallBack != null)
                onItemChangedCallBack.Invoke();
        }

        return true;
    }

    public void RemoveEquipment(int index)
    {
        equipments.RemoveAt(index);
        if (onItemChangedCallBack != null)
            onItemChangedCallBack.Invoke();
    }

    void OnEquip(Equipment equipment)
    {
        if(equipment != null)
        {
            ModifyPlayerParameters(equipment.damageModifier, equipment.movementModifier, equipment.armorModifier);
        }
    }

    void UnEquip(Equipment equipment)
    {
        if(equipment != null)
        {
            ModifyPlayerParameters(-equipment.damageModifier, -equipment.movementModifier, -equipment.armorModifier);
        }
    }

    void UseItem(int index)
    {
        Item item = items[index];
        if (index == 0)
        {
            //Top item is for health items
            print("Using Health Item!");
            AddPlayerHealth(item.Amount);
            items[index] = null;
        }
        else if (index == 1)
        {
            //Bottom item is for temporary movement and jump boost items
            print("using Movement or jump Item");
            if(item.isJumpType == true)
            {
                AddPlayerJumpTemp(item.Amount, item.duration, item);
            }
            else
            {
                AddPlayerMovementTemp(item.Amount, item.duration, item);
            }
            items[index] = null;
        }
        if (onItemChangedCallBack != null)
            onItemChangedCallBack.Invoke();
    }

    void FindPlayer()
    {
        if (PlayerManagerOld.instance.playerInstance != null)
        {
            player = PlayerManagerOld.instance.playerInstance;
            stats = player.GetComponent<PlayerStats>();
            if(stats == null)
            {
                Debug.LogError(player.name + " doesn't contain PlayerStats.cs!");
                return;
            }
        }
        else
        {
            // If there isn't a PlayerManager it will give an error
            Debug.LogError(PlayerManagerOld.instance.name + " Is either not found or its missing!");
            return;
        }
    }

    private void Update()
    {
        if(player == null)
        {
            FindPlayer();
        }
        coinCounter.text = stats.coins.ToString("0");
        if(items.Count > 0)
        {
            if(Input.GetKeyDown(KeyCode.Alpha1) && items[0] != null)
            { 
                UseItem(0);
            }
            else if(Input.GetKeyDown(KeyCode.Alpha2) && items[1] != null)
            {                             
                UseItem(1);
            }
        }
    }

    public void AddPlayerHealth(float amount)
    {
        if (stats != null)
        {
            stats.RestoreHealth(amount);
        }
    }

    public void AddPlayerMovementTemp(float amount, float time, Item item)
    {
        if(stats != null && item.isJumpType == false)
        {
            stats.TemporaryModifyPlayerItemStats(amount, 0f, time);
        }
    }
    public void AddPlayerJumpTemp(float amount, float time, Item item)
    {
        if (stats != null && item.isJumpType == true)
        {
            stats.TemporaryModifyPlayerItemStats(0f, amount, time);
        }
    }

    public void AddPlayerCoinage(float amount)
    {
        if(stats != null)
        {
            stats.CurrencyAdd(amount);
        }
    }

    public void ModifyPlayerParameters(float damage, float movementSpeed, float armor)
    {
        if (stats != null)
        {
            stats.ModifyPlayerEquipmentStats(damage, movementSpeed, armor);
        }
    }
}