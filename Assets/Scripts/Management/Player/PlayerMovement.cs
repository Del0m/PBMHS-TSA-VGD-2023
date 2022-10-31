using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    //Range of the raycasting
    public float range = 100f;
    //Speed of the player moving to a point
    public float movementSpeed = 5;

    private Camera cam;
    private PositionManager posM;
    Vector3 norm;

    // Start is called before the first frame update
    void Awake()
    {
        //Set by default the current position
        norm = this.transform.position;
        //Find the position manager
        GameObject obj = GameObject.FindGameObjectWithTag("Manager");
        //Sets the position manager to the object with the 'Manager' tag
        posM = obj.GetComponent<PositionManager>();
        //Finds the camera object with a tag
        GameObject camObj = GameObject.FindGameObjectWithTag("MainCamera");
        //Sets the camera to the object with the 'MainCamera' tag
        cam = camObj.GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        //When player uses input 
        if (Input.GetMouseButtonDown(0))
        {
            //Check what the player hits
            if(Physics.Raycast(ray, out hit, range) && hit.transform.tag == "pos")
            {
                //Debug.Log(hit.transform.name + " was hit");
                PointCheck tar = hit.transform.GetComponent<PointCheck>();
                if(tar != null && posM != null)
                {
                    //Set point target
                    Vector3 tg = new Vector3(tar.pos.x, 0f, tar.pos.z);
                    //Give info to position manager
                    if (posM.checkForPos(tg))
                    {
                        norm = tg;
                    }
                    else
                    {
                        print("Point is occupied");
                    }
                }
            }
        }
        // Call to move to point
        moveToPoint(norm);
    }

    void moveToPoint(Vector3 target)
    {
        target.y = this.transform.position.y;
        // Check if the var is not null
        if(target != null)
        {
            //Move until there is no distance between player and target
            if (Vector3.Distance(transform.position, target) != 0)
            {
                this.transform.position = Vector3.MoveTowards(this.transform.position, target, movementSpeed * Time.deltaTime);
            }
        }
        else
        {
            //For debugging
            Debug.LogWarning("No target found");
            return;
        }
    }
}
