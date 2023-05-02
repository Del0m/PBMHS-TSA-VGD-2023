using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Bson;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SPManager : PlayerManager
{
    // difficulty variables
    public int level;

    // ui that will change with level
    public TextMeshProUGUI levelGUI;

    public override void SpawnPlayer(PlayerInput input)
    {
        SetPlayer(input.gameObject);
        base.SpawnPlayer(input);

        // set the player to have the first turn, so games don't break.
        input.gameObject.GetComponent<PlayerStats>().turnOrder = 0;

    }
    // set players to singleplayer mode
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
    public override void TransitionGame(int won)
    {

        // update the level the player is playing on
        IncreaseLevel();
        if (won == -1) // if the player won
        {
            StartCoroutine(uiManager.UIPopUp(uiManager.gameOverUI));
        }
        else // if player lost
        {
            {
                StartCoroutine(uiManager.UIPopUp(uiManager.successUI));
            }
        }

    }
    private void UpdateDifficulty()
    {
        multiplier = Math.Pow(multiplier, (1 + level / 3.3));
    }
    public void IncreaseLevel() // make single player game progressively more difficult
    {
        Debug.Log("Updating the level!");

        level = (player[0].GetComponent<PlayerStats>().wins + 1);

        levelGUI.text = "Level: " + level.ToString();
        UpdateDifficulty();
    }
    public void LoadScene(string sceneString)
    {
        Debug.Log("Moving to scene: " + sceneString);
        SceneManager.LoadScene(sceneString);
    }
    public void GameOver() // end the game when the player loses the game 
    {
        //Reset turn order
        turn.SetTurn(0);

        // siginify in single player that player has lost
        var uiManager = GameObject.FindGameObjectWithTag("PlayerUIManager").GetComponent<PlayerUIManager>();

        uiManager.gameOverUI.SetActive(true);
    }
}
