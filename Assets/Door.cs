using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Door : MonoBehaviour // allowing the player to go play the next minigame
{
    public MiniGameManager gameManager;
    public bool isLooking; // boolean to prevent multiple runs of minigame searching
    public TextMeshProUGUI loadingText;

    public GameObject obj;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && !isLooking)
        {
            // checking for player input
            var isActing = collision.GetComponent<PlayerMovement>().acting;

            if(isActing)
            {
                isLooking = true; // start searching for minigames

                // move player onto the next minigame

                StartCoroutine(gameManager.StartMiniGame());
                StartCoroutine(Wait());
            }
        }
    }
    public void Loading() // show that the door is loading a minigame
    {
        if(isLooking)
        {
            loadingText.gameObject.SetActive(true);
        }
        else
        {
            loadingText.gameObject.SetActive(false);
        }
    }

    IEnumerator Wait() // run loading to show when the minigame is being initalized
    {
        Disappear(obj);
        Loading();
        yield return new WaitForSeconds(5f);
        isLooking = false;
        Loading();
    }
    public void Disappear(GameObject obj) // makes text disappear
    {
        obj.SetActive(false);
    }
}
