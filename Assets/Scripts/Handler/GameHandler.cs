//armin delmo, 11/13/22. Purpose of script is to handle minigame startup, grabbing players, starting game, etc.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MiniGameManager
{
    //arrays
    public GameObject[] players;
    private GameObject[] spawns;


    //children objects
    private Camera miniCam;
    // Start is called before the first frame update
    void Start()
    {
        //calling all players, calling all minigame spawns
        players = GameObject.FindGameObjectsWithTag("Player");
        spawns = GameObject.FindGameObjectsWithTag("Minigame Spawn"); // spawn for player to put in if need be.

        //calling children components
        miniCam = this.gameObject.GetComponentInChildren<Camera>(); // should grab camera off of Minigame View.
    }

    // Update is called once per frame
    void Update()
    {
        GetPlayers(players);
    }
}
