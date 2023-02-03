using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    public Settings setting;
    public AudioSource _audio;
    // Update is called once per frame
    private void Start()
    {
        _audio= GetComponent<AudioSource>();
        setting = GameObject.FindGameObjectWithTag("Settings").GetComponent<Settings>();
    }
    void Update()
    {
        _audio.volume = setting.musicVolume; // constantly update music volume
    }
}
