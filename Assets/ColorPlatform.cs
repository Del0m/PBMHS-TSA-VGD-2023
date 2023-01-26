using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPlatform : MonoBehaviour
{
    private Vector2 startingPos; // this position is going to move back when it drops
    [Header("Drop Location")]
    public Transform dropPos; // position to drop to

    public bool dropPlatform; // to be switched in ColorMatch minigame
    // Start is called before the first frame update
    void Start()
    {
        startingPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        MovePlatform();
    }
    void MovePlatform()
    {
        if(dropPlatform)
        {
            transform.position = Vector2.MoveTowards(this.transform.position, dropPos.position, 8 * Time.deltaTime);
        }
        else
        {
            transform.position = Vector2.MoveTowards(this.transform.position, startingPos, 8 * Time.deltaTime);

        }
    }
}
