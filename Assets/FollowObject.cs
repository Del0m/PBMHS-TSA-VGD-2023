using UnityEngine;

public class FollowObject : MonoBehaviour
{
    //NOTE: This script is only intended to be for RunningScamper
    [Header("params")]
   // public float speed = 5f;
    public Vector3 offset;
    public GameObject objToFollow;


    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 z = new Vector3(0, 0, objToFollow.transform.position.z);
        transform.position = (objToFollow.transform.position - z) + offset;
    }
}
