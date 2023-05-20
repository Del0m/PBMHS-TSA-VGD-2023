using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    public GameObject pause;
    private bool isPaused;

    public Settings setting;

    public Slider[] slides;

    private void Start()
    {
        pause = this.gameObject;
        setting = GameObject.FindGameObjectWithTag("Settings").GetComponent<Settings>();

        SetSliders();

    }
    public void SetSliders() // on start up set slides to proper locales
    {
        slides[0].value = setting.masterVolume * 100;
        slides[1].value = setting.musicVolume * 100;
        slides[2].value = setting.soundVolume * 100;
    }

    public void ChangeMaster(Slider slide)
    {
        var volume = slide.value;
        setting.ModifyVolume("masterVolume", (volume / 10));
    }
    public void ChangeMusic(Slider slide)
    {
        var volume = slide.value;
        setting.ModifyVolume("musicVolume", (volume / 10));
    }
    public void ChangeSoundEffect(Slider slide)
    {
        var volume = slide.value;
        setting.ModifyVolume("soundVolume", (volume / 10));
    }

    //following is for pausemenu
    public void PauseGame()
    {
        //stops all time dependent variables in the game

        isPaused = !isPaused;
        if (isPaused == true)
        {
            pause.gameObject.SetActive(true);
            Time.timeScale = 0.0000001f;

            // put eventsystem onto master volume
            var _event = EventSystem.current;
            _event.SetSelectedGameObject(slides[0].gameObject);
        }
        else
        {
            pause.gameObject.SetActive(false);
            Time.timeScale = 1f;
        }

    }
}
