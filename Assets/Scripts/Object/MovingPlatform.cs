using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : Platform
{
    public Transform[] point; // to deduct where the platform will go
    public float speed; // to see how fast the platform will go

    public Transform[] pointStorage; // storing points for minigames, not needed

    int index = 0;
    private void Update()
    {
        MovePlatform();
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        collision.transform.SetParent(null);
    }
    void MovePlatform() // move platform between all points
    {
       // if (point == null) { return; } // stop error spam
        //if(point.Length == 0) { return; } // stop error spam
        
        if(point != null && point.Length > 0 && point[index] != null) //Check the point before updating the transform
        {
            if (Vector2.Distance(transform.position, point[index].position) < 0.02f)
            {
                index++;
                if (index >= point.Length)
                {
                    index = 0;
                }
            }

            transform.position = Vector2.MoveTowards(transform.position, point[index].position, speed * Time.deltaTime);
        }
        else
        {
            return;
        }
    }

}
