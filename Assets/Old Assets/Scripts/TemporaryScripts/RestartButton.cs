using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartButton : MonoBehaviour
{
    public int sceneIndex;
    public void restart()
    {
        print("Restarting!");
        SceneManager.LoadScene(sceneIndex);
    }
}
