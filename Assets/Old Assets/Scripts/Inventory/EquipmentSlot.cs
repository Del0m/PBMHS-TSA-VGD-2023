using UnityEngine.UI;
using UnityEngine;

public class EquipmentSlot : MonoBehaviour
{
    public Image icon;

    Equipment equipment;

    public void AddEquipment(Equipment newEquipment)
    {
        equipment = newEquipment;

        icon.sprite = equipment.icon;
        icon.enabled = true;
    }

    public void ClearSlot()
    {
        equipment = null;

        icon.sprite = null;
        icon.enabled = false;
    }
}
