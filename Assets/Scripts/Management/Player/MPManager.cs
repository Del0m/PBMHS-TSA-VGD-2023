// armin delmo, MPManager.cs; this will manage the multiplayer side for the players.
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class MPManager : PlayerManager
{
    public void StartMultiplayer() // Move player in select location
    {
        for (int i = 0; i < player.Count; i++)
        {
            var plr = player[i].gameObject;
            var playerSpawn = plr.GetComponent<PlayerStats>().turnOrder;

            plr.transform.position = spawn[playerSpawn].position; // move players to map
            plr.GetComponent<PlayerMovement>().GameSwitch(false); // prevent player moving
        }
        turn.SetTurn(0);
        turn.uiManager.InitalizeUI();

    }
    public void Multiplayer(PlayerInput input)
    {
        input.gameObject.transform.position = waitSpawn.transform.position;

        // allowing player movement
        var movement = input.gameObject.GetComponent<PlayerMovement>();

        movement.rb = input.gameObject.GetComponent<Rigidbody2D>();
        movement.GameSwitch(true);
    }

    public virtual IEnumerator StartGame() // start the game, run DisableJoin
    {
        yield return new WaitForSeconds(2);
        DisableJoin(); // turn off players from being able to join the game
        yield return new WaitForSeconds(5); // wait 5 seconds before going on

        turn.SetTurn(0); //allow players to begin doing their turns
        // ^ to be changed to after cutscene overlooking map

        // multiplayer procedure begins here
        StartMultiplayer();

    }
    private void DisableJoin() // disable players joining the game.
    {
        manager.DisableJoining(); // disables joining from the players end
    }
    public override void SpawnPlayer(PlayerInput input) // adds to player array for minigames
    {

        var spawnPlayer = input.gameObject; // grabbing object of player


        player.Add(spawnPlayer); // add player into the count

        spawnPlayer.GetComponent<PlayerStats>().turnOrder = (player.Count - 1); // give the player their turn
    }
    public override void SetPlayer(GameObject spawnPlayer) // change the players color
    {
        spawnPlayer.GetComponentInChildren<SpriteRenderer>().color = playerColor.ElementAt(player.Count - 1);
    }
}
