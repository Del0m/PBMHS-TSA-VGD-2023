/* armindelmo MovementManager.cs 10 / 2 / 22
  The purpose of this program is to move the player from one tile to another.
*/
using UnityEngine;

public class MovementManager : MonoBehaviour
{
    //tile and tile related goods
    [Header("Debug")]
    public Transform[] tile;

    public TurnManager turnManage;

    //player array
    //public GameObject[] players;

    private void Awake() // for manager initalization
    {
        this.gameObject.tag = "Movement Manager"; //  allow gameobjects to find this script.
    }

    void Start()
    {
        tile = InitializeTiles();
    }

    Transform[] InitializeTiles()
    {
        //will grab all tiles on map, and will find length, and categorize them
        var tileAmount = GameObject.FindGameObjectsWithTag("Tile");
        var localTile = new Transform[tileAmount.Length];

        for(int i = 0; i < tileAmount.Length; i++) // loop through all tiles, put them in neat array :)
        {
            if (GameObject.Find(i.ToString()) == null)
            {
                Debug.LogError("Issue in finding tile " + i);
            }
           localTile[i] = GameObject.Find(i.ToString()).transform;
        }
        //Debug.LogError(localTile[1]);
        return localTile;
    }
    public Transform CallTile(int position, int increase, int playerID)
    {
        var newPosition = position + increase; // adding their current position and rolled die

        //giving them the specific tile

        return tile[newPosition].GetComponent<Tile>().playerPositions[playerID]; // return the player's position of the tile
    }
    public Transform CallTile(int position) // to be called when leaving a game
    {
        return tile[position];
    }
    public BuffObject CollectBuff(int position) // collects items from the tile to give to the player
    {
        var buff = tile[position].gameObject.GetComponent<Tile>().buffToGive;
        return buff;
    }
}
