using UnityEngine;

public class FollowObject : MonoBehaviour
{
    //NOTE: This script is only intended to be for RunningScamper
    [Header("params")]
   // public float speed = 5f;
    public Vector3 offset;
    public GameObject objToFollow;


    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = new Vector3(objToFollow.transform.position.x + offset.x, transform.position.y, transform.position.z);
    }
}
