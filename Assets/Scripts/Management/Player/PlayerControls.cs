//armindelmo Playermanager.cs
//the purpose of this program is to find controllers connected, then print output when found.


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerControls : MonoBehaviour
{
    public bool isPressed;

    public PlayerManager managerScript;

    //tell manager script to put player in array
    private void Start()
    {
        managerScript.playerCount += 1; // increase player count
        managerScript.Invoke("GetPlayers", 2); // run method at start
    }
    void Update()
    {
        //detect if player has pressed key
        if (isPressed != false)
        {
            Debug.Log("Button is Pressed on gamepad!");
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 1, this.transform.position.z); // make new player
        }
    }
    public void OnPress(InputAction.CallbackContext ctx) => isPressed = ctx.ReadValueAsButton(); // returns true when button pressed
    

}
