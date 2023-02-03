using UnityEngine;

public class FollowObject : MonoBehaviour
{
    //NOTE: This script is only intended to be for RunningScamper
    [Header("params")]
   // public float speed = 5f;
    public Vector3 offset;
    public GameObject objToFollow;

    private void Start()
    {
        if(objToFollow == null)
        {
            objToFollow = GameObject.FindGameObjectWithTag("MainCamera");
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(objToFollow != null)
        {
            transform.position = new Vector3(objToFollow.transform.position.x + offset.x, transform.position.y, transform.position.z);
        }
        else
        {
            Debug.LogWarning("The camera not found!");
            return;
        }
    }
}
