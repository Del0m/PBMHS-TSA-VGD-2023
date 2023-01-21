// armin delmo; sound.cs; 1/21/23
// the purpose of this script is to modify the volume of the sound it is on
// this should be put on music scripts or ongoing sounds that are present in the game
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour
{
    // to collect volume
    public Settings setting;
    public float volume;
    private AudioSource source;
    [Header("Volume Type")]

    public string volumeType = "musicVolume";
    private void Start()
    {
        GetObjects(); // grab settings for volume
        GetVolume(volumeType);
    }
    void GetObjects()
    {
        if (!setting) // if setting is unselected in scene
        {
            try
            {
                setting = GameObject.FindGameObjectWithTag("Settings").GetComponent<Settings>();
            }
            catch (System.Exception)
            {
                Debug.LogError("Settings not found! Attempting to instantiate!");
                // setting object to grab settings script
                var settingObject = Instantiate(Resources.Load("Settings", typeof(GameObject)) as GameObject);
                setting = settingObject.GetComponent<Settings>();

                throw;
            }

        }
        if(source == null)
        {
            source = this.GetComponent<AudioSource>(); // from the game object
        }    
    }
    public void GetVolume(string volumeType)
    {
        volume = setting.musicVolume * setting.masterVolume; // master volume as a multiplier
        source.volume = volume;
    }
}
