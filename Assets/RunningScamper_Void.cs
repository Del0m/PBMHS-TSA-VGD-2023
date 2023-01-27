using UnityEngine;

public class RunningScamper_Void : MonoBehaviour
{
    private GameObject cageObj;

    private void Start()
    {
        cageObj = GameObject.Find("Cage");
        if(cageObj == null)
        {
            Debug.LogError("Couldn't find Cage!");
            return;
        }
        else
        {
            Debug.Log("Found the cage!");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.transform.position = cageObj.transform.position;
            Debug.Log("Player got sent to the cage!"); // Insert kinky thought here
        }
    }
}
