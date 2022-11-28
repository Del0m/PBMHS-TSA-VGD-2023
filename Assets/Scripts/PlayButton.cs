using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayButton : MonoBehaviour
{
    [Header("Specify Which Scene to load")]
    public int sceneIndex;


    public void PlayGame()
    {
        SceneManager.LoadScene(sceneIndex);
        print("Loading to scene number" + sceneIndex);
    }
}
