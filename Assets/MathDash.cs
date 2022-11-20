using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; // for sensing input
public class MathDash : GameHandler
{
    private GameObject[] plr;
    private bool ongoing;

    // Start is called before the first frame update
    void Start()
    {
        plr = this.gameObject.GetComponent<GameHandler>().players; // fill player array
        ongoing = true;
        TakeTurn();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void TakeTurn() // lets plr[i] make their turn by listening for input.
    {
        var playerWent = false;
        while (ongoing == true)
        {
            for(int i = 0; i > plr.Length-1; i++)
            {
                while (playerWent == false)
                {
                    var control = plr[i].GetComponent<PlayerControls>().controls;
                    if(control.miniGamePlay.MiniGameTest.WasPerformedThisFrame())
                    {
                        playerWent = true;
                        Debug.Log("Running!");
                    }

                }
            }
        }
        
    }

}
