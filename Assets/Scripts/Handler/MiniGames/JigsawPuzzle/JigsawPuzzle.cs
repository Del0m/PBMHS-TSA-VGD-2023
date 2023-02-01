// armin delmo; JigsawPuzzle.cs
// the purpose of this program is to construct a jigsaw puzzle game where players grab the pieces to their puzzle
// to construct it before everyone else.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
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
        StartCoroutine(StartGame(true,true,true)); // teleport players to game; topdown game
        StartCoroutine(FormBoard());
    }
    public override IEnumerator EndGame(int winner)
    {

        for (int i = 0; i < player.Length; i++) // for loop to bring players back to normal movement
        {
            player[i].GetComponent<PlayerMovement>().GameSwitch(false, false, false);
        }
        return base.EndGame(winner);
    }
    public void CheckEnd(int correctPieceCount) // check to see if user has correctly solved the puzzle
    {
        if(correctPieceCount >= 9)
        {
            StartCoroutine(EndGame(boardPrefab.GetComponent<Jigsaw_Board>().pieceID)); // end the game from GameHandler
        }
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
            var plrBoard = Instantiate(boardPrefab, puzzleLocation[i].position, new Quaternion(0, 0, 0, 0), this.gameObject.transform);
            plrBoard.SetActive(true); // initalize the board onto the game
            plrBoard.transform.position = new Vector3(plrBoard.transform.position.x, plrBoard.transform.position.y, 0f);
            plrBoard.GetComponent<Jigsaw_Board>().pieceID = i;

            // for loop to initalize all the jigsaw holes
            for(int j = 0; j < plrBoard.GetComponent<Jigsaw_Board>().slot.Length; j++)
            {
                // disgusting... but it works :-)
                plrBoard.GetComponent<Jigsaw_Board>().slot[j].GetComponent<JigsawHole>().pieceID = j; // piece type
                plrBoard.GetComponent<Jigsaw_Board>().slot[j].GetComponent<JigsawHole>().imageID = i; // picture type
            }


            SelectImage(); // selects image puzzle will base itself on.
            StartCoroutine(plrBoard.GetComponent<Jigsaw_Board>().BoardStartup(i)); // run startup function to begin game
        }
    }
    public void SpawnPuzzle(GameObject parent, int plrOrder, Transform firstPiece) // spawns puzzle pieces, uses parent to define puzzle images
    {
        //slotting in first puzzle piece to let players know which one they're making.
        var jigsawPiece = Instantiate(piecePrefab, new Vector3(firstPiece.position.x, firstPiece.position.y), new Quaternion(0, 0, 0, 0), this.gameObject.transform); // make jigsaw piece spawn on map
        jigsawPiece.SetActive(true); // show to game
        // mod jigsaw piece to have image
        jigsawPiece.GetComponent<SpriteRenderer>().sprite = jigsawImage[0];

        jigsawPiece.GetComponent<JigsawPiece>().imageID = plrOrder; // ID image
        jigsawPiece.GetComponent<JigsawPiece>().pieceID = 0; // to prevent incorrect puzzle pieces being put in
        jigsawPiece.GetComponent<HoldableItem>().canPickUp = true; // allow item to be picked by player

        // for loop to create pieces for puzzle
        for (int i = 1; i < 9; i++)
        {
            RandomizePosition(); // randpos to put jigsaw puzzle around
            jigsawPiece = Instantiate(piecePrefab, randPos, new Quaternion(0, 0, 0, 0), this.gameObject.transform); // make jigsaw piece spawn on map
            jigsawPiece.SetActive(true); // show to game
            // mod jigsaw piece to have image
            jigsawPiece.GetComponent<SpriteRenderer>().sprite = jigsawImage[i];

            jigsawPiece.GetComponent<JigsawPiece>().imageID = plrOrder; // ID image
            jigsawPiece.GetComponent<JigsawPiece>().pieceID = i; // to prevent incorrect puzzle pieces being put in
            jigsawPiece.GetComponent<HoldableItem>().canPickUp = true; // allow item to be picked by player
        }
    }


}
