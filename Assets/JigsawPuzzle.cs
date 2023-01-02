// armin delmo; JigsawPuzzle.cs
// the purpose of this program is to construct a jigsaw puzzle game where players grab the pieces to their puzzle
// to construct it before everyone else.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JigsawPuzzle : GameHandler
{
    // for jigsaw placement / objects
    public Transform[] puzzleLocation;
    public GameObject piecePrefab;
    public GameObject boardPrefab;

    // random vector to put piece in
    private Vector2 randPos;

    // jigsaw image users must produce
    public GameObject imageCollection; // gameobject that will store all, will be instantiated and removed.
    private Sprite[] jigsawImage;

    private void Start()
    {
        StartCoroutine(TeleportPlayers(true)); // teleport players to game; topdown game
        StartCoroutine(FormBoard());
    }
    void RandomizePosition() // this runs to randomize the position in the arena
    {
        //getting dimensions of arena
        var xLow = border[0].transform.position.x + 4;
        var xHigh = border[2].transform.position.x - 4;

        var yLow = border[1].transform.position.y + 4;
        var yHigh = border[3].transform.position.y - 2;

        //returning random values to spawn target in.
        randPos = new Vector2(Random.Range(xLow, xHigh), Random.Range(yLow, yHigh));
        return;

    }
    private void SelectImage() // purpose is to select random image collection from ones available.
    {
        var images = Instantiate(imageCollection); // to grab images
        jigsawImage = images.GetComponent<ImageScript>().ImageSelection(); // grab randomImage.
        Destroy(images, 1f); // rid imageCollection
    }

    private IEnumerator FormBoard() // instantiates board
    {
        yield return new WaitForSeconds(3);

        var playerCount = GameObject.FindGameObjectsWithTag("Player").Length; // detects plr count

        // for loop to spawn players' boards in their respective location.
        for(int i = 0; i < playerCount; i++)
        {
            Debug.Log("spawning board");
            var plrBoard = Instantiate(boardPrefab, puzzleLocation[i].position, new Quaternion(0, 0, 0, 0));
            plrBoard.transform.position = new Vector3(plrBoard.transform.position.x, plrBoard.transform.position.y, 0f);


            SelectImage(); // selects image puzzle will base itself on.
            StartCoroutine(plrBoard.GetComponent<Jigsaw_Board>().BoardStartup(i)); // run startup function to begin game
        }
    }
    public void SpawnPuzzle(GameObject parent, int plrOrder) // spawns puzzle pieces, uses parent to define puzzle images
    {
        // for loop to create pieces for puzzle
        for(int i = 0; i < 9; i++)
        {
            RandomizePosition(); // randpos to put jigsaw puzzle around
            var jigsawPiece = Instantiate(piecePrefab, randPos, new Quaternion(0, 0, 0, 0)); // make jigsaw piece spawn on map

            // mod jigsaw piece to have image
            jigsawPiece.GetComponent<SpriteRenderer>().sprite = jigsawImage[i];

            jigsawPiece.GetComponent<JigsawPiece>().imageID = plrOrder; // ID image
            jigsawPiece.GetComponent<JigsawPiece>().pieceID = i; // to prevent incorrect puzzle pieces being put in
        }
    }


}
