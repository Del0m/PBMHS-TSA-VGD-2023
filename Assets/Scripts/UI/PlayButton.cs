using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEditor;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class PlayButton : MonoBehaviour
{
    [Header("Specify Which Scene to load")]
    public int gameScene; // fix later
    [Header("Menu")]
    public GameObject back; // UI to see where player would like to go
    public GameObject play; // title of game

    public GameObject multiplayer;
    public GameObject singleplayer;
    public AudioSource aud;

    public EventSystem _event;

    [Header("Text")]
    public GameObject playText;
    public GameObject choiceText;

    private void Start() // grab event system
    {
        _event = EventSystem.current;
    }
    public void PlayGame(string sceneString) // to bring player into game scene
    {
        SceneManager.LoadScene(sceneString);
    }
    public void BringUI(bool setUI) // this brings the multiplayer or single player ui for the player to choose what to play
    {
        var setUIAlpha = 255;
        var oppSetUIAlpha = 0;

        if(!setUI)
        {
            setUIAlpha = 0;
            oppSetUIAlpha = 255;
        }

        back.GetComponent<Button>().interactable = setUI;
        back.GetComponentInChildren<TextMeshProUGUI>().alpha = setUIAlpha;

        play.GetComponent<Button>().interactable = !setUI;
        play.GetComponentInChildren<TextMeshProUGUI>().alpha = oppSetUIAlpha;

        multiplayer.GetComponent<Button>().interactable = setUI;
        multiplayer.GetComponentInChildren<TextMeshProUGUI>().alpha = setUIAlpha;

        singleplayer.GetComponent<Button>().interactable = setUI;
        singleplayer.GetComponentInChildren<TextMeshProUGUI>().alpha = setUIAlpha;

        playText.SetActive(!setUI);
        choiceText.SetActive(setUI); 

        if(setUI)
        {
            _event.SetSelectedGameObject(back);
        }
        else
        {
            _event.SetSelectedGameObject(play);
        }
        if (!aud.isPlaying) // check to see if music is playing yet
        {
            // play music
            aud.Play();
        }

    }
}
