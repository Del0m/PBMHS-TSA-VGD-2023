using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManagerOld : MonoBehaviour
{
    public static PlayerManagerOld instance;

    private void Awake()
    {
        if (instance != this)
        {
            instance = this;
        }

        //Finds both the player and camera instance
        setInstances();

        //Finds the player by tag and sets it to Game object class for other scripts to reference the player
        if (playerInstance.tag == "Player" && playerInstance != null)
        {
            print("Player found!");
        }
        else
        {
            Debug.LogError("Player not found in scene!");
        }
        //find camera, used for teleportation, nothing else...
        if (cameraInstance.tag == "MainCamera" && cameraInstance != null)
        {
            print("camera found!");
        }
        else
        {
            Debug.LogError("Camera not found in scene!");
        }

        if (playerInstance == null || cameraInstance == null)
        {
            setInstances();
        }
    }

    public void setInstances()
    {
        playerInstance = GameObject.FindGameObjectWithTag("Player");
        cameraInstance = GameObject.FindGameObjectWithTag("MainCamera");
    }

    public GameObject playerInstance;
    public GameObject cameraInstance;

}
