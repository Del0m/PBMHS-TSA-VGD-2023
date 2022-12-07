using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine;

public class GameManager_Temporary : MonoBehaviour
{
    public static GameManager_Temporary instance;

    public GameObject pause;
    private bool isPaused;
    
    void Start()
    {
        if(instance != this)
        {
            instance = this;
        }
 
        //find pause menu, close pause menu, print pause menu
        pause = GameObject.FindGameObjectWithTag("Pause");
        if(pause != null)
        {
            pause.SetActive(false);
        }
        else
        {
            pause = GameObject.FindGameObjectWithTag("Pause");
            pause.SetActive(false);
        }
        isPaused = false;
        Debug.Log(pause);
    }
    private void Update()
    {
        if (instance != this)
        {
            instance = this;
            Debug.Log("Reset Instance!");
            return;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //enter the pausemenu
            PauseGame();
        }
        else if(pause == null)
        {
            pause = GameObject.FindGameObjectWithTag("Pause");
            pause.SetActive(false);
        }
    }
    public void restartLevel()
    {
        // Is called to show a game over    
        SceneManager.LoadScene(2);
    }

    public void EndOfGame()
    {
        // Called to show a end screen
        StartCoroutine(ChangeScene());
    }

    IEnumerator ChangeScene()
    {
        yield return new WaitForSeconds(1.2f);
        SceneManager.LoadScene(1);
    }
    //following is for pausemenu
    public void PauseGame()
    {
        //stops all time dependent variables in the game
        
        isPaused = !isPaused;
        if(isPaused == true)
        {
            pause.gameObject.SetActive(true);
            Time.timeScale = 0.0000001f;
            print("pausing!");
        }
        else
        {
            pause.gameObject.SetActive(false);
            Time.timeScale = 1f;
        }

    }
    public void QuitGame()
    {
        Debug.Log("Disabling the Application!");
        Application.Quit();
    }
}
