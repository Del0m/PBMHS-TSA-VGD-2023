using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; // for the purposes of manipulating player controls
using TMPro; // to manipulate ui

[RequireComponent(typeof(Rigidbody))]
[RequireComponent (typeof(PlayerScore))]
public class PlayerMovement : MonoBehaviour
{
    //Range used to make sure player doesn't select points very far away from them
    //Maximum range to find the closest point
    public float range = 8f;
    //Speed of the player moving to a point
    public float movementSpeed = 5;

    //Minimum range to find the closest point
    public float minRange = 3f;

    private Camera cam;
    [HideInInspector]
    public Vector3 norm;

    //Call randomizer script
    public int playerTurn { get; private set; } = 0;

    //Private vars
    private PlayerScore ps;
    private Renderer mesh;
    private Randomizer rand;

    private GameObject[] pointObjects;
    private List<PointCheck> points = new List<PointCheck>();
    private bool hasReachedPoint = false;

    //player controls
    private PlayerInput playerControl;
    private Controls controls;

    private PlayerControls playerScript;

    //ui elements
    private GameObject turnUI;

    void addMapPoints()
    {
        //Find the points and put into array
        pointObjects = GameObject.FindGameObjectsWithTag("pos");

        for (int i = 0; i < pointObjects.Length; i++)
        {
            if (i == pointObjects.Length)
                break;
            points.Add(pointObjects[i].GetComponent<PointCheck>());
        }

        print(points.Count);
        //Put points into a list
    }

    // Start is called before the first frame update
    void Awake()
    {
        //Set by default the current position
        norm = this.transform.position;
        //Sets the camera to the object with the 'MainCamera' tag
        GameObject camObj = GameObject.FindGameObjectWithTag("MainCamera");
        cam = camObj.GetComponent<Camera>();
        //Set the 'Randomizer' class 
        rand = this.GetComponent<Randomizer>();
        //Set the 'PlayerScore' class
        ps = this.GetComponent<PlayerScore>();
        //Set the Player's renderer
        mesh = this.GetComponent<Renderer>();

        addMapPoints();

        //set player controls
        controls = new Controls();
        playerControl = this.GetComponent<PlayerInput>();
        playerScript = this.GetComponent<PlayerControls>();

        StartCoroutine(waitForInit());

        turnUI = GameObject.Find("Moves Left");


    }

    void changePlayerColor()
    {
        switch (ps.playerIndex)
        {
            case 0:
                mesh.material.color = Color.blue;
                break;
            case 1:
                mesh.material.color = Color.red;
                break;
            case 2:
                mesh.material.color = Color.green;
                break;
            case 3:
                mesh.material.color = Color.yellow;
                break;
        }
    }

    IEnumerator waitForInit()
    {
        yield return new WaitForSeconds(1.5f);
        // Debug | on awake set a random number between 1 - 6
        // if (rand != null)
        //      playerTurn = rand.DiceRoll(1, 6);
        changePlayerColor();
        playerTurn = 5;
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

        // Move player to point when there is a change on the target
        moveToPoint(norm);
    }

    //debug and can be removed
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, range);
    }

    bool hasRan = false;
    public void userInput()
    {
        // Automatically move to closes point
        if(hasRan == false && hasReachedPoint == true) // not allow player to roll several times.
        {
            StartCoroutine(MakeMove(2));
        }

    }

    void checkClosePoint()
    {
        foreach(PointCheck p in points)
        {
            if(Vector3.Distance(this.transform.position, p.pos) < range && p.correspondingPlayerIndex == ps.playerIndex && !p.hasSeenPlayer)
            {
                if(Vector3.Distance(this.transform.position, p.pos) > minRange)
                {
                    Vector3 t = new Vector3(p.pos.x, 0f, p.pos.z);

                    float dist = Vector3.Distance(this.transform.position, p.pos);

                    if(checkPath(t, dist) && !p.isOccupied)
                    {
                        norm = t;
                    }
                    break;
                }
            }
        }
    }

    bool checkPath(Vector3 tart, float dist)
    {
        // Use the player's gfx to raycast a direction to check for entities in the path

        RaycastHit hit;

        tart.y = transform.position.y;

        //Rotate Player to point
        transform.LookAt(tart);

        //Check his path for entities
        if(Physics.Raycast(this.transform.position, transform.TransformDirection(Vector3.forward), out hit, dist, LayerMask.GetMask("Entity"))){
            //debug
            print("Entity is in my path");

            return false;
        }

        //Check distance from the player
        if(Vector3.Distance(this.transform.position, tart) > range)
        {
            print(tart + " is very far away from the player");
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
                hasReachedPoint = false;
            }
            else
            {
                hasReachedPoint = true;
            }
        }
        else
        {
            //For debugging
            Debug.LogWarning("No target found");
            return;
        }
    }
    IEnumerator MakeMove(int seconds)
    {
        hasRan = true;
        var roll = Random.Range(1, 7);

        playerTurn = roll; // updates player movement to move this amount of times.
        var movesLeft = roll; // defines how many moves are left to do for the player
        while (playerTurn > 0)
        {
            if(playerTurn == 0)
            {
                break;
            }

            Debug.Log("Moving!");

            turnUI.GetComponent<TextMeshProUGUI>().text = "Moves Left: " + movesLeft;
            checkClosePoint();
            yield return new WaitForSeconds(seconds);
            movesLeft--;
        }
        hasRan = false;
    }
}
