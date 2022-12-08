using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class EquipmentPickUp : ItemPickUpProperties
{
    public Equipment equipment;

    bool addEquipment = false;

    PlayerStats stats;

    private GameObject equipmentInfo = null;
    private Text equipmentInfoText = null; //[]Use this to also show conformation text
    //Add objects to be reference for adding text to show info

    private void OnValidate()
    {
        if(gameObject.name != "EquipmentPickUp_Template")
        {
            addEquipmentToRender();
        }
    }

    void findText()
    {
        if (equipmentInfo == null)
        {
            GameObject canvas = this.gameObject.transform.Find("Equipment_canvas").gameObject;
            if (canvas != null)
            {
                GameObject itemTextObj = canvas.transform.Find("Equipment_Info").gameObject;
                if (itemTextObj != null)
                {
                    equipmentInfo = itemTextObj;
                    equipmentInfoText = equipmentInfo.GetComponent<Text>();
                    print("Found and set the item info text");
                }
            }
            else
            {
                Debug.LogError(canvas.name + " not found!");
            }
        }
    }

    private void Start()
    {
        setObjectSize();
        FindPlayer();
        findText();
        setTextToFilter();
        SetCollider();
        if (gameObject.name != "EquipmentPickUp_Template")
        {
            addEquipmentToRender();
        }
    }

    void SetCollider()
    {
        if (equipment != null)
        {
            int index = (int)equipment.colliderType;
            if (index == 0) //Circle collider 2D
            {
                //disable box collider
                Collider2D col = this.GetComponent<Collider2D>();
                if (col != null)
                {
                    col.enabled = false;
                }

                //enable circle collider and set parameters
                CircleCollider2D circl = this.GetComponent<CircleCollider2D>();
                if (circl != null)
                {
                    circl.enabled = true;
                    circl.radius = equipment.radius;
                }
            }
            else if (index == 1) //Box collider 2D
            {
                //disable circle collider
                CircleCollider2D circl = this.GetComponent<CircleCollider2D>();
                if (circl != null)
                {
                    circl.enabled = false;
                }

                //enable circle collider and set parameters
                BoxCollider2D col = this.GetComponent<BoxCollider2D>();
                if (col != null)
                {
                    col.enabled = true;
                    col.size = equipment.size;
                }
            }
        }
    }

    void setObjectSize()
    {
        if(equipment != null)
        {
            transform.localScale = equipment.objectSize;
            return;
        }
    }

    public void addEquipmentToRender()
    {
        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null && equipment != null)
        {
            spriteRenderer.sprite = equipment.icon;
        }
        else
        {
            Debug.LogWarning(this.name + "does not have a SpriteRenderer componenet || no Equipment has been set!");
            return;
        }
    }

    public override void InteractAction()
    {
        base.InteractAction();

        if(hasInteracted == true)
        {
            if(equipmentInfo != null)
            {
                equipmentInfo.gameObject.SetActive(true);
            }

            if (equipment != null)
            {
                addEquipment = true;
            }
            else
            {
                Debug.LogError("No Equipment has been set");
                addEquipment = false;
            }
        }
        else
        {
            if(equipmentInfo != null)
            {
                equipmentInfo.gameObject.SetActive(false);
                setTextToFilter();
            }
            addEquipment = false;
        }
    }

    void setTextToFilter()
    {
        if(equipment != null && equipmentInfoText != null)
        {
            if (isShopItem)
            {
                equipmentInfoText.text = "damage: " + equipment.damageModifier + "   " + "Armor: " + equipment.armorModifier + "   " + "Movement: " + equipment.movementModifier + "   " + "Cost:" + equipment.value;
            }
            else
            {
                equipmentInfoText.text = "damage: " + equipment.damageModifier + "   " + "Armor: " + equipment.armorModifier + "   " + "Movement: " + equipment.movementModifier;
            }
        }
    }

    void LateUpdate()
    {
        if(equipment != null && player != null && equipmentInfo != null)
        {
            PickUp();
            setTextToFilter();
        }
        else if(equipmentInfo == null)
        {
            findText();
            FindPlayer();
        }
    }

    void PickUp()
    {
        if (!isShopItem)
        {
            if (Input.GetKeyDown(KeyCode.E) && addEquipment == true && player != null)
            {
                PlayerInventory.instance.AddEquipment(equipment);
                Debug.Log("Picked up " + equipment.name);
                addEquipment = false;
                Destroy(gameObject);
            }
        }
        else
        {
            if (player != null)
            {
                stats = player.GetComponent<PlayerStats>();

                equipmentInfoText.text = "Press E to confirm purchase";

                StartCoroutine(beforeBuyCoolDown());

                if (Input.GetKeyDown(KeyCode.E) && addEquipment == true && stats.coins >= equipment.value && stats != null && readyToBuy == true)
                {
                    stats.CurrencySubtract(equipment.value);
                    PlayerInventory.instance.AddEquipment(equipment);
                    Debug.Log("Picked up " + equipment.name);
                    addEquipment = false;
                    Destroy(gameObject);
                }
                else if (stats == null)
                {
                    Debug.LogError(player.name + " does not have a PlayerStats.cs!");
                    return;
                }
            }
        }
    }

    bool readyToBuy = false;

    IEnumerator beforeBuyCoolDown()
    {
        yield return new WaitForSeconds(0.25f);
        readyToBuy = true;
        StopCoroutine(beforeBuyCoolDown());
    }
}
