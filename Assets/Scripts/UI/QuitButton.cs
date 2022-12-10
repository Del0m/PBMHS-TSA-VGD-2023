using UnityEngine;

public class QuitButton : MonoBehaviour
{
    public void QuitGame()
    {
        print("Quit!");
        Application.Quit();
    }
}
