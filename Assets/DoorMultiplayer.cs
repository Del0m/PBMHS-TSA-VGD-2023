using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DoorMultiplayer : MonoBehaviour
{
    public bool starting; // boolean to play the start of the game
    public PlayerManager manager;
    public TextMeshProUGUI text; 

    private void Update()
    {
        
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !starting)
        {

            // checking for player input
            var isActing = collision.GetComponent<PlayerMovement>().acting;
            Debug.Log(isActing);
            if (isActing)
            {
                if (manager.player.Length < 2)
                {
                    text.text = "You must have more than one player to start!";
                    return;
                }
                starting = true; // start searching for minigames

                // start the player game
                manager.StartMultiplayer();
                text.text = "Please wait, loading map!";

            }
        }
    }
}
