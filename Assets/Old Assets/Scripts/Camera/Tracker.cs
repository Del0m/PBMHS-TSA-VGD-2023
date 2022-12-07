using UnityEngine;

public class Tracker : MonoBehaviour
{
    private Transform trackedObject;
    public float updateSpeed = 3;
    public Vector3 trackingOffset;
    private Vector3 offset;

    void Start()
    {
        if(trackedObject == null)
        {
            FindObject();
            offset = (Vector3)trackingOffset;
            offset.z = transform.position.z - trackedObject.position.z;
        }
    }

    void FindObject()
    {
        if(PlayerManagerOld.instance.playerInstance != null)
        {
            trackedObject = PlayerManagerOld.instance.playerInstance.transform;
        }
        else
        {
            return;
        }
    }

    void Update()
    {
        if (trackedObject == null)
        {
            FindObject();
        }
        else
        {
            float speed = updateSpeed;
            // If the player is far behind the player in the y direction, speed the camera up to catch up
            if (trackedObject.position.y < transform.position.y ||
                    trackedObject.position.y > transform.position.y)
            {
                // If the camera is far behind the player in the x direction, do not speed the camera up
                if (!((trackedObject.position.x - 1) <= transform.position.x))
                {
                    if (!((trackedObject.position.x + 1) >= transform.position.x))
                    {
                        speed *= 2f;
                    }
                }
                else if(!((trackedObject.position.y - 1) <= transform.position.y))
                {
                    if (!((trackedObject.position.y + 1) >= transform.position.y))
                    {
                        speed *= 2f;
                    }
                }
               

            }
            transform.position = Vector3.MoveTowards(transform.position,
                trackedObject.position + offset, speed * Time.deltaTime);
        }
    }
}
