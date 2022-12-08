using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class EndGameTrigger : MonoBehaviour
{
    [Header("Parameters")]
    public GameObject transitionIU;
    public int sceneIndex;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            transitionIU.SetActive(true);
            StartCoroutine(LoadEndScene());
        }
    }

    IEnumerator LoadEndScene()
    {
        yield return new WaitForSeconds(3.5f);
        SceneManager.LoadScene(sceneIndex);
    }
}
