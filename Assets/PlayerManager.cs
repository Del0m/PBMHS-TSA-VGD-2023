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
    private TurnManager turn;
    private void Start()
    {
        manager = GetComponent<PlayerInputManager>();
        turn = GameObject.FindGameObjectWithTag("Turn Manager").GetComponent<TurnManager>();
    }

    IEnumerator StartGame() // start the game, run DisableJoin
    {
        yield return new WaitForSeconds(2);
        DisableJoin(); // turn off players from being able to join the game
        yield return new WaitForSeconds(5); // wait 5 seconds before going on

        turn.SetTurn(1); //allow players to begin doing their turns
        // ^ to be changed to after cutscene overlooking map
    }
    private void DisableJoin() // disable players joining the game.
    {
        manager.DisableJoining(); // disables joining from the players end
    }
}
