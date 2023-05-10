using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MPCameraControl : CameraControl // purpose of script is to not players roll their dice UNTIL the camera gets to them
{
    [Header("Multiplayer Variables")]
    public TurnManager turnScript;
    
    public void Start()
    {
        turnScript = GameObject.FindGameObjectWithTag("Turn Manager").GetComponent<TurnManager>();
    }
    public override bool AllowMovement()
    {
        Vector2 currentPosition = new Vector2(this.gameObject.transform.position.x, this.gameObject.transform.position.y);
        Vector2 destVector = new Vector2(destination.position.x, destination.position.y);
        if(Vector2.Distance(currentPosition,destVector) < 10) // can't do entire vector, considers z axis
        {
            Debug.Log(target);
            return true;
        }
        this.gameObject.transform.position = new Vector3(target.x, target.y, -100f); // speed up the transition
        return false;
    }
}
