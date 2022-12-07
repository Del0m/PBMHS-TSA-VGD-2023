using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public Transform itemsParent;
    public Transform equipmentParent;

    PlayerInventory inventory;

    InventorySlot[] itemSlots;
    EquipmentSlot[] equipmentSlots;

    void Start()
    {
        inventory = PlayerInventory.instance;

        itemSlots = itemsParent.GetComponentsInChildren<InventorySlot>();
        equipmentSlots = equipmentParent.GetComponentsInChildren<EquipmentSlot>();

        inventory.onItemChangedCallBack += UpdateUI;
    }

    void UpdateUI()
    {
        Debug.Log("updating UI!");
        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (i < inventory.items.Count && inventory.items[i] != null)
            {
                itemSlots[i].Additem(inventory.items[i]);
            }
            else
            {
                itemSlots[i].ClearSlot();
            }
        }
        for(int i = 0; i < equipmentSlots.Length; i++)
        {
            if(i < inventory.equipments.Count && inventory.equipments[i] != null)
            {
                equipmentSlots[i].AddEquipment(inventory.equipments[i]);
            }
            else
            {
                equipmentSlots[i].ClearSlot();
            }
        }
    }

    void LateUpdate()
    {
        FindInventory();
    }

    void FindInventory()
    {
        //In case inventory not found, update to find it
        if(inventory == null)
        {
            inventory = PlayerInventory.instance;
            inventory.onItemChangedCallBack += UpdateUI;
        }
    }

}
