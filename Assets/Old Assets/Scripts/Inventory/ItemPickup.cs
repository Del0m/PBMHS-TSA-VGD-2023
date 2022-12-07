using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]

public class ItemPickup : ItemPickUpProperties
{
    public Item item;

    bool add = false;
    int ItemIndex;
    int ItemCollider;

    PlayerStats stats;

    private GameObject itemInfo = null;
    private Text itemInfoText = null; //Use this to also show conformation text

    private void OnValidate()
    {
        if (gameObject.name != "ItemPickUp_Template")
        {
            addItemToRender();
        }
    }

    void findText()
    {
        if(itemInfo == null && ItemIndex != 2)
        {
            GameObject canvas = this.gameObject.transform.Find("Item_canvas").gameObject;
            if(canvas != null)
            {
                GameObject itemTextObj = canvas.transform.Find("Item_Info").gameObject;
                if (itemTextObj != null)
                {
                    itemInfo = itemTextObj;
                    itemInfoText = itemInfo.GetComponent<Text>();
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
        ItemCollider = (int)item.colliderType;
        ItemIndex = (int)item.itemType;

        setObjectSize();
        FindPlayer();
        SetCollider();
        findText();
        addItemToRender();

        if(itemInfo != null)
        {
            if (isShopItem)
            {
                TextInfoFilterShop();
            }
            else
            {
                TextInfoFilter();
            }
        }
    }

    void SetCollider()
    {
        if(item != null)
        {

            //Debug
            /*
            if(index == 0 && item.radius == 0.035f)
            
                Debug.LogError(item.name + " has collider properties error!");
            }else if(index == 1 && item.size == new Vector2(0.2f, 0.2f))
            {
                Debug.LogError(item.name + " has collider properties error!");
            }
            */
            if(ItemCollider == 0) //Circle collider 2D
            {
                //disable box collider
                BoxCollider2D col = this.gameObject.GetComponent<BoxCollider2D>();
                if(col != null)
                {
                    col.enabled = false;
                    print(item.name + " has disabled box collider!");
                }

                //enable circle collider and set parameters
                CircleCollider2D circl = this.GetComponent<CircleCollider2D>();
                if(circl != null)
                {
                    circl.enabled = true;
                    circl.radius = item.radius;
                }
            }
            else if(ItemCollider == 1) //Box collider 2D
            {
                //disable circle collider
                CircleCollider2D circl = this.GetComponent<CircleCollider2D>();
                if (circl != null)
                {
                    circl.enabled = false;
                    print(item.name + " has disabled circle collider!");
                }

                //enable circle collider and set parameters
                BoxCollider2D col = this.GetComponent<BoxCollider2D>();
                if (col != null)
                {
                    col.enabled = true;
                    col.size = item.size;
                }
            }
        }
    }

    public void addItemToRender()
    {
        if (item != null)
        {
            SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null & item != null)
            {
                spriteRenderer.sprite = item.icon;
            }
        }
        else
        {
            add = false;
            return;
        }
    }

    public override void InteractAction()
    {
        base.InteractAction();

        if(hasInteracted == true && item != null)
        {
            if(itemInfo != null)
            {
                itemInfo.gameObject.SetActive(true);
            }

            if (item.isImmediatelyConsumable)
            {
                PickUpConsumable();
                add = false;
            }
            else
            {
                add = true;
            }
 
        }else if(hasInteracted != true)
        {
            if (itemInfo != null)
            {
                itemInfo.gameObject.SetActive(false);
                if (itemInfo != null)
                {
                    if (isShopItem)
                    {
                        TextInfoFilterShop();
                    }
                    else
                    {
                        TextInfoFilter();
                    }
                }
            }
            readyToBuy = false;
            add = false;
        }
    }

    void LateUpdate()
    {
        if (item != null && player != null && itemInfo != null)
        {
            addItem();
        }
        else if(player == null)
        {
            FindPlayer();
        }
    }

    void setObjectSize()
    {
        if(item != null)
        {
            transform.localScale = item.objectSize;
            return;
        }
    }

    void TextInfoFilter()
    {
        if(item.itemType == itemClass.health)
        {
            //Change text to refer that item is a health based item
            itemInfoText.text = "Healing Amount: " + item.Amount;
        }
        else if(item.itemType == itemClass.movementOrJump)
        {
            if (item.isJumpType)
            {
                itemInfoText.text = "Jump Amount: " + item.Amount;
            }
            else
            {
                itemInfoText.text = "Extra Movement Speed: " + item.Amount;
            }
        }
    }

    void TextInfoFilterShop()
    {
        if (ItemIndex == 0)
        {
            //Change text to refer that item is a health based item
            itemInfoText.text = "Healing Amount: " + item.Amount + "   " + "Cost: " + item.value;
        }
        else if (ItemIndex == 1)
        {
            if (item.isJumpType)
            {
                itemInfoText.text = "Jump Amount: " + item.Amount + "   " + "Cost: " + item.value;
            }
            else
            {
                itemInfoText.text = "Extra Movement Speed: " + item.Amount + "   " + "Cost: " + item.value;
            }
        }
    }

    void PickUpConsumable()
    {
        Debug.Log("Picked up " + item.name);

        if(ItemIndex == 2)
        {
            PlayerInventory.instance.AddPlayerCoinage(item.value);
        }
        else if(ItemIndex == 1)
        {
            PlayerInventory.instance.AddPlayerHealth(item.Amount);
        }
        Destroy(gameObject);
    }

    void addItem()
    {

        //[] Change the way the input is done to this function
        if (isShopItem != true && Input.GetKeyDown(KeyCode.E) && add == true && ItemIndex != 2)
        {
            //Pick up item and add to inventory
            PlayerInventory.instance.AddItem(item);
            Debug.Log("Picked up " + item.name);
            add = false;
            Destroy(gameObject);
        }
        else if(isShopItem != false && Input.GetKeyDown(KeyCode.E) && add == true && ItemIndex != 2)
        {
            if (player != null)
            {
                itemInfoText.text = "Press E to confirm purchase";
                stats = player.GetComponent<PlayerStats>();

                StartCoroutine(beforeBuyCoolDown());

                if (stats.coins >= item.value && stats != null && Input.GetKeyDown(KeyCode.E) && readyToBuy == true)
                {
                    stats.CurrencySubtract(item.value);
                    PlayerInventory.instance.AddItem(item);
                    Debug.Log("Picked up " + item.name);
                    add = false;
                    Destroy(gameObject);
                }
                else if (stats == null)
                {
                    Debug.LogError(player.name + " does not have a PlayerStats.cs!");
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
