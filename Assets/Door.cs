using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Door : MonoBehaviour // allowing the player to go play the next minigame
{
    public MiniGameManager gameManager;
    public bool isLooking; // boolean to prevent multiple runs of minigame searching
    public TextMeshProUGUI loadingText;

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


    IEnumerator Wait() // run loading to show when the minigame is being initalized
    {
        yield return new WaitForSeconds(5f);
        isLooking = false;
    }
}
