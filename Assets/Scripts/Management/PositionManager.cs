using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PositionManager : MonoBehaviour
{
    public GameObject[] pointObj;

    //Added manually, each point will return a position in Vector3
    public GameObject[,] pointObjects2D = new GameObject[3, 3];
    /*
        [2, 0, 1, 2]
        [1, 0, 1, 2]
        [0, 0, 1, 1]
    */

    // Input from entities

    // Find and tag the entity position depending the input

    // Checks for the transform
    void findObjects()
    {
        pointObj = GameObject.FindGameObjectsWithTag("pos");

        //Sort 1D array in order by name from least to greatest
        Array.Sort(pointObj, (a, b) => a.name.CompareTo(b.name));

        //Sort into 2D array
        for(int c = 0; c <= pointObjects2D.GetLength(0) - 1; )
        {
            for (int r2 = 0; r2 <= pointObjects2D.GetLength(1) - 1;)
            {
                for(int r1 = 0; r1 <= pointObj.Length;)
                {
                    if(r1 == pointObj.Length)
                    {
                        //Exit out of the whole function
                        return;
                    }

                    //Set the object 1d array to the 2d array
                    pointObjects2D[c, r2] = pointObj[r1];

                    print("1D Array : " + pointObj[r1].GetComponent<PointCheck>().pos + " Name : " + pointObj[r1].name);
                    print("2D array : " + "Column : " + c + " Row : " + r2 + " Name : " + pointObjects2D[c, r2].name);

                    //Checks for the 2d array length of the column 
                    if(r2 == pointObjects2D.GetLength(1) - 1)
                    {
                        r2 = 0;
                        c++;
                        r1++;
                    }
                    else if(r2 != pointObjects2D.GetLength(1) - 1)
                    {
                        r1++;
                        r2++;
                    }
                }
            }
        }
    }

    //[] Return true or false if the point is or not occupied
    public bool checkForPos(Vector3 entityPos) // Input from entities
    {
        //Check in list where position is in columns and rows
        for(int c = 0; c <= pointObjects2D.GetLength(0) - 1;)
        {
            for(int r = 0; r <= pointObjects2D.GetLength(1) - 1;)
            {
                if (c > pointObjects2D.GetLength(0) - 1)
                {
                    //Exit when amount of columns exceeds the lenght of columns of the 2D array
                    print("Entity is not in the grid or is out of bounds");
                    break;
                }
                else if (pointObjects2D[c, r].transform.position.x == entityPos.x && pointObjects2D[c, r].transform.position.z == entityPos.z)
                {
                    //Check the point 
                    PointCheck pc = pointObjects2D[c, r].GetComponent<PointCheck>();
                    if (pc != null )
                    {
                        //Check for the variable that tells if the point is occupied by another entity
                        if (!pc.isOccupied)
                        {
                            //Point is not occupied and print where the player is now
                            print("entity is at : " + pointObjects2D[c, r].name + " in Row = " + (r + 1) + " Column = " + (c + 1));
                            return true;
                        }
                        else //The point is occupied by an entity
                        {
                            print(pointObjects2D[c, r].name + " Is occupied by an entity!");
                            return false;
                        }
                    } else
                    {
                        Debug.LogError(pointObjects2D[c, r].name + " Does not contain PointCheck.cs!");
                        return false;
                    }
                }

                // Checks every row and moves to the nexy column when all rows are checked
                if (r == pointObjects2D.GetLength(1) - 1)
                {
                    r = 0;
                    c++;
                }
                else if (r != pointObjects2D.GetLength(1) - 1)
                {
                    r++;
                }     
            }
        }

        return false;
    }

    // Start is called before the first frame update
    void Start()
    {
        findObjects();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
