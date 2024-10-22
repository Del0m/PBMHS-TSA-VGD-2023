// armin delmo; 12/27/22; Jigsaw_Board.cs
// the purpose of this program is to grab the puzzle pieces and put their part of the picture onto them

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Jigsaw_Board : MonoBehaviour
{
    public JigsawPuzzle puzzleScript; // parent script; calls functions
    public int pieceCount = 9;
    public int pieceID; // to find if piece is correct; transfers to puzzle piece
    public GameObject[] slot;

    public int playerPuzzle; // meant to be what the player's puzzle is

    public int piecesCorrect; // amount of pieces correct for user
    public IEnumerator BoardStartup(int plrOrder) // runs functions to start up the player's board in the game
    {
        playerPuzzle = plrOrder;
        PieceSpawn(plrOrder);
        yield return new WaitForSeconds(2);

    }
    void PieceSpawn(int plrOrder) // run SpawnPuzzle
    {
        //randomly select puzzleImage array here [ ]
        puzzleScript.SpawnPuzzle(this.gameObject, plrOrder, slot[0].transform); // spawning the puzzle pieces in here.

    }
    public void ChangeCorrect(int value) // update the amount of pieces slottedd correctly in the game
    {
        piecesCorrect += value; // update value
        var scorePlayer = slot[0].GetComponent<JigsawHole>().imageID;
        puzzleScript.gameScore[scorePlayer]+=value;
        // run the function to check to see if it is time to end the game
        if(piecesCorrect == pieceCount)
        {
            puzzleScript.CheckEnd(piecesCorrect, playerPuzzle); // procedure to end game
        }

    }
}
