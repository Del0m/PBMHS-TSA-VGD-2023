// armin delmo; 12/27/22; Jigsaw_Board.cs
// the purpose of this program is to grab the puzzle pieces and put their part of the picture onto them

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Jigsaw_Board : MonoBehaviour
{
    public JigsawPuzzle puzzleScript; // parent script; calls functions
    public Image[] puzzleImage; // segments of images to be put
    public int pieceCount = 9;
    public string pieceID; // to find if piece is correct; transfers to puzzle piece
    public GameObject[] slot;

    public IEnumerator BoardStartup() // runs functions to start up the player's board in the game
    {
        PieceSpawn();
        yield return new WaitForSeconds(2);

    }
    void PieceSpawn() // run SpawnPuzzle
    {
        //randomly select puzzleImage array here [ ]

        //
        puzzleScript.SpawnPuzzle(this.gameObject); // spawning the puzzle pieces in here.

    }
}
