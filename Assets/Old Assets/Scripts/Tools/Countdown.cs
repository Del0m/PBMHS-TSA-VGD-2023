using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Countdown : MonoBehaviour
{
    [Header("Coundown parameters")]
    public float secondsToCountDown;
    public int sceneIndex = 0;

    void Start()
    {
        StartCoroutine(changeScene());
    }

    IEnumerator changeScene()
    {
        yield return new WaitForSeconds(secondsToCountDown);
        SceneManager.LoadScene(sceneIndex);
        StopAllCoroutines();
    }
}
