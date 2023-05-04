// armin delmo; EndGame.cs; to be ran when the game has ended to send an individual to another game, or out of it.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour
{
    public void SwitchScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}
