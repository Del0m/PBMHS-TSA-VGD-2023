using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEditor.SearchService;
using UnityEngine.EventSystems;
using UnityEngine.WSA;
using UnityEditor;

public class PlayButton : MonoBehaviour
{
    [Header("Specify Which Scene to load")]
    public int gameScene; // fix later
    [Header("Menu")]
    public GameObject playChoice; // UI to see where player would like to go
    public GameObject title; // title of game
    public void PlayGame() // to bring player into game scene
    {
        SceneManager.LoadScene(gameScene);
    }
    public void BringUI(bool setUI) // this brings the multiplayer or single player ui for the player to choose what to play
    {
        playChoice.SetActive(setUI);
        title.SetActive(!setUI);

    }
}
