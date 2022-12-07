using System.Collections;
using UnityEngine;

public class BossCheck : MonoBehaviour
{
    /// <summary>
    /// This script detects the player that can be accesed by enemies that are
    /// colliding it, to make the enemy that there is a player in their area
    /// </summary>

    public GameObject bossDoor;

    public bool PlayerIsInArea { get; private set; }
    
    private bool wait = true;
    private GameObject enemyBoss;

    private AudioManager audioManager;

    private bool stopReplay = false;

    void Start()
    {
        //Find AudioManager by instance
        audioManager = FindObjectOfType<AudioManager>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Boss") || collision.gameObject.tag == "Enemy")
        {
            enemyBoss = collision.gameObject;
            //print("Boss found!");
        }
        else if (collision.gameObject.tag == "Player" && enemyBoss != null)
        {
            PlayerIsInArea = true;
            if (audioManager != null && !stopReplay)
            {
                audioManager.StopMain("Theme");
                audioManager.Play("Boss Music");
                stopReplay = true;
            }
            else
            {
                //Debug.LogWarning("Finding the audio manager again");
                audioManager = FindObjectOfType<AudioManager>();
                return;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerIsInArea = false;
        }
    }

    void Update()
    {
        CheckBoss();
    }

    void CheckBoss()
    {
        if (bossDoor != null)
        {
            StartCoroutine(waitForBossFound());
            if(wait == false)
            {
                if(enemyBoss == null)
                {
                    bossDoor.SetActive(false);
                    if (audioManager != null && stopReplay == true)
                    {
                        //Put back theme song
                        audioManager.StopMain("Boss Music");
                        audioManager.Play("Theme");
                        stopReplay = false;
                    }
                }
                else
                {
                    bossDoor.SetActive(true);
                }
            }
        }
    }

    IEnumerator waitForBossFound()
    {
        yield return new WaitForSeconds(2f);
        wait = false;
        StopCoroutine(waitForBossFound());
    }
}
