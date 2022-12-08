//armin delmo
//the purpose of this script is to disable players joining the game once the game has begun.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    //defining player array
    private GameObject[] player;
    private PlayerInputManager manager;

    private void Start()
    {
        manager = GetComponent<PlayerInputManager>();
    }

    IEnumerator StartGame() // start the game, run DisableJoin
    {
        yield return new WaitForSeconds(2);
        DisableJoin(); // turn off players from being able to join the game
        yield return new WaitForSeconds(5); // wait 5 seconds before going on
    }
    private void DisableJoin() // disable players joining the game.
    {
        manager.DisableJoining(); // disables joining from the players end
    }
}
