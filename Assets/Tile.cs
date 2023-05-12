using System.Collections;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [Header("Rewards")]
    public BuffObject buffToGive;
    public int winsToGive;

    public Transform[] playerPositions;

    //UI
    //[Header("UI Params")]

    MiniGameManager mgm;
    PlayerManager pm;
    private PlayerControls player;
    void Start()
    {
        this.gameObject.tag = "Tile";
        mgm = GameObject.FindGameObjectWithTag("Mini Game Manager").GetComponent<MiniGameManager>();
        pm = GameObject.FindGameObjectWithTag("Player Manager").GetComponent<PlayerManager>();
        if(mgm == null){
            Debug.LogError("Can't find mini game manager in scene!");  
            return;     
        }else if(pm == null){
            Debug.LogError("Can't find player manager!");
            return;
        }else if(playerPositions.Length < 0 && playerPositions.Length != 4){
            Debug.LogError("Not all player position in tile were set!");
            return;
        }
    }

    bool checkPlayer(GameObject player){

        if(pm != null && player != null){

            playerIndex = -1;

            for(int i = 0; i < pm.player.Count; i++){
                if(pm.player[i] == player){
                    Debug.Log("Found player in player manager!");
                    playerIndex = i;
                    player = null;
                    return true;
                }
            }

            if(playerIndex == -1){
                Debug.LogError("Player doesn't exist in player manager!");
                return false;
            }
        }

        Debug.Log("can't find Player or the player manager");
        return false;
    }

    //local use only
    int playerIndex;
    }

