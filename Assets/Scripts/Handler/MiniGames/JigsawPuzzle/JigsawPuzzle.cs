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


    // jigsaw image users must produce
    public GameObject imageCollection; // gameobject that will store all, will be instantiated and removed.
    private Sprite[] jigsawImage;

    [Header("Single Player Variables")]
    public int time; // time to solve the puzzle


    private void Start()
    {
        // calling manager for UI updates
        uiManager = GameObject.FindGameObjectWithTag("PlayerUIManager").GetComponent<PlayerUIManager>();
        

        StartCoroutine(StartGame()); // teleport players to game; topdown game
        StartCoroutine(FormBoard());
    }

    public override void IncreaseDifficulty() // make time shorter to slot pieces
    {
        base.IncreaseDifficulty();
        time = (int)(time / multiplier); 

        // check if the game is single player to see if a minimum score should be added
        if(plrManage.singlePlayer)
        {
            minimumToWin = 9;
            Debug.Log("minimumToWin = " + minimumToWin);
        }
    }
    public override IEnumerator PreGameRoutine() // adding a timer to the minigame in singleplayer
    {
        IncreaseDifficulty();
        yield return StartCoroutine(base.PreGameRoutine());
    }
    public override IEnumerator StartGame()
    {
        StartCoroutine(base.StartGame());

        yield return new WaitForSeconds(3); // countdown
        yield return StartCoroutine(gameUI.Timer(time)); // running the timer

        // run loop to bring players
        for (int i = 0; i < player.Count; i++)
        {
            var playerMovement = player[i].GetComponent<PlayerMovement>();

            playerMovement.GameSwitch(true, true, true);
        }
        yield return null; 
    }

    public void CheckEnd(int correctPieceCount) // check to see if user has correctly solved the puzzle
    {
        if(correctPieceCount >= 9)
        {
            StartCoroutine(EndGame(boardPrefab.GetComponent<Jigsaw_Board>().playerPuzzle)); // end the game from GameHandler
        }
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
            jigsawPiece = Instantiate(piecePrefab, RandPosition(), new Quaternion(0, 0, 0, 0), this.gameObject.transform); // make jigsaw piece spawn on map
            jigsawPiece.SetActive(true); // show to game
            // mod jigsaw piece to have image
            jigsawPiece.GetComponent<SpriteRenderer>().sprite = jigsawImage[i];

            jigsawPiece.GetComponent<JigsawPiece>().imageID = plrOrder; // ID image
            jigsawPiece.GetComponent<JigsawPiece>().pieceID = i; // to prevent incorrect puzzle pieces being put in
            jigsawPiece.GetComponent<HoldableItem>().canPickUp = true; // allow item to be picked by player
        }
    }


}
