using UnityEngine;
/*
 * purpose of program; make a lock for grid maps in order to keep player inside, fighting until he dies or he kills all enemies, simple as that. 
 * 
 * NEEDS TO HAVE ENEMIES AS A CHILD OF GRID_MAP_REFERENCE  
 */
public class GridLocker : MonoBehaviour
{
    //for the purpose of closing the grid map so the player can fight the enemies and not run like a coward.
    public GameObject[] arenaWalls;
    public int enemiesLeft;
    public bool playerInArena = false;
    public bool hasRan = false;

    //adds enemies to counter, and turns on bool necessary for opening and closing arena
    private void OnTriggerEnter2D(Collider2D collide)
    {
        if (collide.gameObject.CompareTag("Enemy"))
        {
            enemiesLeft += 1;
        }
        else if (collide.gameObject.CompareTag("Player"))
        {
            playerInArena = true;
        }
    }

    private void Update()
    {
        //close the gates when the player is in grid map w/ enemies
        if (playerInArena == true && enemiesLeft > 0 && hasRan == false)
        {
            for (int i = 0; i <= arenaWalls.Length -1; i++)
            {
                arenaWalls[i].SetActive(true);
                Debug.Log("Closing wall " + arenaWalls[i] + " now!");
                hasRan = true;
            }         
        }
        //open gates when player killed all enemies
        else if (playerInArena == true && enemiesLeft <= 0 &&hasRan == true)
        {
            for (int i = 0; i <= arenaWalls.Length-1; i++)
            {
                arenaWalls[i].SetActive(false);
                Debug.Log("Opening wall " + arenaWalls[i] + " now!");
                hasRan = false;
            }
        }
    }
}
