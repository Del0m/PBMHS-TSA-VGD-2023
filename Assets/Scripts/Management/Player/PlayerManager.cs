//armin delmo
//the purpose of this script is to disable players joining the game once the game has begun.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{

    private PlayerInputManager manager; // disable joining mechanics
    private TurnManager turn; // start up turns

    [Header("Player")]
    public Transform[] spawn; // spawn player in correct spot
    public GameObject[] player; // player append variable
    private void Start()
    {
        manager = GetComponent<PlayerInputManager>();
        turn = GameObject.FindGameObjectWithTag("Turn Manager").GetComponent<TurnManager>();
    }
    public void PlayerArray(PlayerInput input) // adds to player array for minigames
    {
        Debug.Log("Appending Player");
        player = player.Append(input.gameObject).ToArray();

    }
    public void SinglePlayer(PlayerInput input)
    {
        var stat = input.gameObject.GetComponent<PlayerStats>();

        stat.singlePlayer = true;
        
        input.gameObject.transform.position = new Vector3(input.gameObject.transform.position.x, input.gameObject.transform.position.y, 5);

        // gameswitch to make jumps exists

        var movement = input.gameObject.GetComponent<PlayerMovement>();

        movement.rb = input.gameObject.GetComponent<Rigidbody2D>();
        movement.GameSwitch(false);
    }
    public void MultiPlayer(PlayerInput input) // Move player in select location
    {
        // get player turn order to spawn them in correct location
        var playerOrder = input.gameObject.GetComponent<PlayerStats>().turnOrder;
        
        input.gameObject.transform.position = spawn[playerOrder].position;
    }

    public IEnumerator StartGame() // start the game, run DisableJoin
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
    public void GameOver()
    {
        // siginify in single player that player has lost
        var uiManager = GameObject.FindGameObjectWithTag("PlayerUIManager").GetComponent<PlayerUIManager>();

        uiManager.gameOverUI.SetActive(true);
    }
}
