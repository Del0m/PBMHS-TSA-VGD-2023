using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SinglePlayerManager : MonoBehaviour
{
    public TextMeshProUGUI levelGUI;
    [Header("Stats")]
    public int level; // level the player is on
    public double multiplier; // multiplier to make games more difficult

    public void UpdateDifficulty()
    {
        if(multiplier == 1)
        {
            multiplier = 1.1;
        }
        multiplier = Math.Pow(multiplier, (1+level/3));
    }
    public void IncreaseLevel() // make single player game progressively more difficult
    {
        level++;
        levelGUI.text = "Level: " + level.ToString();
        UpdateDifficulty(); 
    }
    public void LoadScene(string sceneString)
    {
        Debug.Log("Moving to scene: " + sceneString);
        SceneManager.LoadScene(sceneString);
    }
    
}
