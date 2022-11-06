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
    public Vector3 norm { private get; set; }

    //Call randomizer script
    public int playerTurn { get; private set; } = 0;
    private Randomizer rand;

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
        //Set the 'Randomizer' class 
        rand = this.GetComponent<Randomizer>();
        StartCoroutine(waitForInit());
    }

    IEnumerator waitForInit()
    {
        yield return new WaitForSeconds(2);
        // Debug | on awake set a random number between 1 - 6
        if (rand != null)
            playerTurn = rand.DiceRoll(1, 6);
        print(playerTurn);
    }

    // Function can be called from other scripts
    public void deduceTurn(int amount, Vector3 newPos)
    {
        playerTurn -= amount;
        //debug
        print("Player has " + playerTurn + " turns");

        //Makes the player stay on the point where his turn ended
        if (playerTurn <= 0) 
        {
            print("Player got stuck in " + newPos);
            norm = newPos;
        }

        //To make sure is called once
        return;
    }

    // Update is called once per frame
    void Update()
    {
        //debug and can be removed
        Debug.DrawLine(transform.position, transform.forward * range, Color.yellow);

        // When player has turns, the player is able to give input
        //[] Make a different type of input if on controller
        if(playerTurn > 0)
        {
            userInput();
        }
        // Move player to point when there is a change on the target
        moveToPoint(norm);
    }

    void userInput()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        //When player uses input 
        if (Input.GetMouseButtonDown(0))
        {
            //Check what the player hits
            if (Physics.Raycast(ray, out hit) && hit.transform.gameObject.layer == LayerMask.NameToLayer("dir"))
            {
                //Debug.Log(hit.transform.name + " was hit");
                PointCheck tar = hit.transform.GetComponent<PointCheck>();
                if (tar != null && posM != null)
                {
                    //Set point target
                    Vector3 tg = new Vector3(tar.pos.x, 0f, tar.pos.z);
                    //Give info to position manager
                    if (checkPath(tg) && checkForDiagnal(tg) && posM.checkForPos(tg))
                    {
                        norm = tg;
                    }
                }
            }
        }
    }

    bool checkPath(Vector3 tart)
    {
        // Use the player's gfx to raycast a direction to check for entities in the path

        RaycastHit hit;

        tart.y = transform.position.y;

        //Rotate Player to point
        transform.LookAt(tart);

        //Check his path for entities
        if(Physics.Raycast(this.transform.position, transform.TransformDirection(Vector3.forward), out hit, range, LayerMask.GetMask("Entity"))){
            //debug
            print("Entity is in my path");

            return false;
        }

        // there is not entity in path so it's clear to move to that point
        return true;
    }

    bool checkForDiagnal(Vector3 pos)
    {
        if(pos.x == this.transform.position.x)
        {
            //Is diagonal on the x
            return true;
        }
        else if(pos.z == this.transform.position.z)
        {
            //Is diagonal on the z
            return true;
        }
        return false;
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
