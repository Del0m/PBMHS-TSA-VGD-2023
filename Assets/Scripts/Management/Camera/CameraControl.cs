using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Apple;

public class CameraControl : MonoBehaviour
{
    private Camera cam; // camera that will move

    private Transform destination; // destination to move camera

    // variables to zoom camera

    private int multiplier; // for zooming; faster / slower
    private bool toZoom; // ask if need to zoom

    private int outerFov; //what to zoom the camera out to.
    private int innerFov; // what to zoom the camera in to.
    private int fov; // what the fov should currently be at

    [Header("Game Mode")]
    public bool singlePlayer; // disables translate camera update.
    private void Awake() // set camera to itself
    {
        cam = this.gameObject.GetComponent<Camera>();
    }
    public void TeleportCamera(Vector3 dest, int camSize){
        cam.gameObject.transform.position = new Vector3(dest.x, dest.y, -100);
        cam.orthographicSize = camSize;

    }
    public IEnumerator ModifyCamera(Transform newDest, int m, int i, int o) // move camera to new location
    {
        if(newDest != null){
            destination = newDest; // only reason why this exists is b/c i want to keep this var private.

            //following are abbrivieated varaiables to modify the camera moving.
            multiplier = m;
            outerFov = o;
            innerFov = i;

            toZoom = true; // zoom out when not at destination
            fov = outerFov; // set fov to zoom out

            while(Vector2.Distance(this.gameObject.transform.position, destination.position) > 10) // hold until at destination
            {
                Debug.Log("Waiting until end destination...");
                yield return new WaitForSeconds(1);
            }
            Debug.Log("Huzzah!!");
            toZoom = false; // zoom back in once destination has been reached
            fov = innerFov; // set fov to zoom in.
        }
    }

    public void setCamUpdate(bool enable){
        camMovement = enable;
        Debug.Log("Setting cam update to " + camMovement);
    }

    public void forgetDestination(){
        destination = null;
        Debug.Log("Forgetting last destination");
    }

    private bool camMovement = false;
    
    // Update is called once per frame; moves camera to intended location
    void Update()
    {
        if(!singlePlayer && camMovement && destination != null) // not needed in single player
        {
            TranslateCamera(multiplier); // shift camera
            ZoomCamera(toZoom, fov, multiplier); // zoom camera.
        }
    }
    
    private void TranslateCamera(int factor) // move camera through x-y axis.
    {
        Debug.Log("Moving!");
        Vector2 target = Vector2.MoveTowards(this.transform.position, destination.position, Time.deltaTime * factor);
        this.transform.position = new Vector3(target.x, target.y, -100f);
    }
    private void ZoomCamera(bool zoom, int view, int factor) // changes field of view on the camera.
    {
        print(zoom);
        if(zoom == true && cam.orthographicSize < view) // for zooming out
        {
            cam.orthographicSize += Time.deltaTime * factor;
            Debug.Log("Zooming out!");
        }
        else if(zoom == false && cam.orthographicSize > view) // zooming back in
        {
            cam.orthographicSize -= Time.deltaTime * factor;
            Debug.Log("Zooming in!");
        }
    }
}
