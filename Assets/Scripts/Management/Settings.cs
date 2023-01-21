// armin delmo; 1/17/23
// purpose of program is to hold settings for player to modify for more enjoyable experience
using System.Collections;
using System.Collections.Generic;
using Unity.Profiling;
using UnityEngine;

public class Settings : MonoBehaviour
{
    public float soundVolume;
    public float musicVolume;
    public float masterVolume;
    int fps; 
    private void Start()
    {
        LoadSettings(); // load the player's settings for volume
        SetFrameRate(fps); // set frame rate for end-user
    }
    public void SetFrameRate(int frame) // sets target frame rate for user
    {
        PlayerPrefs.SetInt("fps", frame);
        Application.targetFrameRate = fps;

        LoadSettings();
    }
    public void ModifyVolume(string setting, float amount) // for the purposes of altering volume for the player
    {
        PlayerPrefs.SetFloat(setting, amount);
        LoadSettings(); // to update the changes made
    }
    public void LoadSettings()
    {
        soundVolume = PlayerPrefs.GetFloat("soundVolume", 1f);
        musicVolume = PlayerPrefs.GetFloat("musicVolume", 1f);
        masterVolume = PlayerPrefs.GetFloat("masterVolume", 1f);
        fps = PlayerPrefs.GetInt("fps", 60);

        PlayerPrefs.Save(); // save all settings done
    }
}
