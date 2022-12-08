using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayerStats : EntityStats
{
    //Economy
    public float coins; //{ get; private set; }

    [Header("Player Parameters")]
    public float defaultPlayerDamage;
    public float defaultPlayerMovementSpeed;
    public float defaultPlayerJump;

    private float currentPlayerDamage;
    private float currentPlayerMovementSpeed;
    private float currentPlayerJump;

    private PlayerControllerOld controller;
    private PlayerCombat combat;

    [Header("scene Manage & Gameplay parameters")]
    public int gameOverSceneIndex;
    public GameObject transitionUI;

    private void Awake()
    {
        //[] Apply the player parameters to the player controller
        currentPlayerDamage = defaultPlayerDamage;
        currentPlayerMovementSpeed = defaultPlayerMovementSpeed;
        defaultPlayerJump *= 100f;
        currentPlayerJump = defaultPlayerJump;

        controller = gameObject.GetComponent<PlayerControllerOld>();
        combat = gameObject.GetComponent<PlayerCombat>();
        if(combat != null && controller != null)
        {
            controller.playerSpeed = currentPlayerMovementSpeed;
            controller.jumpPower = currentPlayerJump;
            combat.damage = currentPlayerDamage;
        }
    }

    private GameObject healthbar;

    public void setHealthBar(GameObject obj)
    {
        healthbar = obj;
    }

    public override void Die()
    {
        base.Die();

        //Call healthbar to reset

        if(healthbar != null)
        {
            //Reset health bar, to not show that it is going invertedly 
            HealthCalculator cal = healthbar.gameObject.GetComponent<HealthCalculator>();
            if(cal != null)
            {
                cal.resetBar = true;

                print("Health Bar reseted!");

                if(cal.resetBar == true)
                {
                    gameObject.SetActive(false); // Makes the player invisible, but still in the scene
                }
            }
        }

        //Call GameOver screen and call transition UI
        transitionUI.SetActive(true);
        SceneManager.LoadScene(gameOverSceneIndex);
    }

    public void ModifyPlayerEquipmentStats(float damage, float movementSpeed, float armor)
    {
        currentPlayerDamage += damage;
        currentPlayerMovementSpeed += movementSpeed;
        Armor += armor;

        controller.playerSpeed = currentPlayerMovementSpeed;
        combat.damage = currentPlayerDamage;
    }
    public void TemporaryModifyPlayerItemStats(float movementSpeed, float jump, float time)
    {
        if(jump != 0f)
        {
            currentPlayerJump = currentPlayerJump + jump * 100f;
        }
        currentPlayerMovementSpeed += movementSpeed;
        controller.playerSpeed = currentPlayerMovementSpeed;
        controller.jumpPower = currentPlayerJump;
        Debug.Log("Added temporary item stats");
        StartCoroutine(returnToDefault(time));
    }

    public IEnumerator returnToDefault(float time)
    {
        yield return new WaitForSeconds(time);
        currentPlayerMovementSpeed = defaultPlayerMovementSpeed;
        currentPlayerJump = defaultPlayerJump;
        controller.playerSpeed = currentPlayerMovementSpeed;
        controller.jumpPower = currentPlayerJump;
        Debug.Log("Reseted stats");
        StopAllCoroutines();
    }

    public void CurrencyAdd(float amount)
    {
        // procedure that adds coins to the player
        coins = coins + amount;
        Debug.Log("Added " + coins + " coins to the player's wallet!");
    }

    public void CurrencySubtract(float amount)
    {
        coins = coins - amount;
        Debug.Log("Deducted " + amount + " coins from the user's wallet!");
    }
}
