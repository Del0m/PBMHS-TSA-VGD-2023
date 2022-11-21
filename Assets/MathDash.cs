using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; // for sensing input
public class MathDash : GameHandler
{
    string rightMove = "A";
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
             // check every frame to see if correct move is made
    }

    public override void GradeMove(string move)
    {
       if(move == rightMove)
        {
            Debug.Log("The Right move has been made!");
        }
        else
        {
            Debug.Log("Incorrect!");
        }

    }

}
