using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    public GameHandler game; // controls the end game function for the player
    public TextMeshProUGUI timeLeft;

    [Header("Moddable Variables")]
    public TextMeshProUGUI textElement;

    public void ModifyText(string word) // change the ui text for the player
    {
        textElement.text = word;
    }
    public IEnumerator Timer(int time) // counts down to zero, will notify the game through EndGame() function
    {
        for(int i = 0; i < time; i++)
        {
            timeLeft.text = "" + (time - i);
            yield return new WaitForSeconds(1f); // wait one second before incrementing the time again
        }
        Debug.Log("running the end game coroutine!");
        StartCoroutine(game.EndGame(game.CheckWinner()));
    }
}
