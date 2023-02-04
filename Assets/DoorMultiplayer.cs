using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DoorMultiplayer : MonoBehaviour
{
    public bool starting; // boolean to play the start of the game
    public PlayerManager manager;
    public TextMeshProUGUI text; 

    public CameraControl cam;

    public GameObject blankScreen;

    [Header("cam params")]
    public Transform initialDestination;
    public int inZoom;
    public int outZoom;
    public int followingSpeed;

    public float timer = 5f;

    void Start(){
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraControl>();
        if(blankScreen == null){
            Debug.LogError("Blank Screen is not set to " + this.name);
            return;
        }

        //Either way turn off the blank screen
        blankScreen.SetActive(false);
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

                StartCoroutine(transitionToMap());

            }
        }
    }

    IEnumerator transitionToMap(){


        //Set camera params
        cam.TeleportCamera(initialDestination.transform.position, 20);

        //Call camera to go to a specified position
        cam.setCamUpdate(true);

        
        yield return new WaitForSeconds(timer); // debug the time before release

        //The camera should be on the players
        cam.setCamUpdate(false);

        Debug.Log("Stoppped the Cam");

    }
}
