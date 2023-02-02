using System.Collections;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [Header("Params")]
    public tileType tType;
    public buffType bType;

    public float buffAmount;

    public Transform[] playerPositions;

    //UI
    //[Header("UI Params")]

    [Header("Debug")]
    public MiniGameManager mgm;
    public PlayerManager pm;
    public int timeToloadSettings = 3;
    // Start is called before the first frame update
    void Awake()
    {
        this.gameObject.tag = "Tile";
        mgm = GameObject.FindGameObjectWithTag("Mini Game Manager").GetComponent<MiniGameManager>();
        pm = GameObject.FindGameObjectWithTag("PlayerManager").GetComponent<PlayerManager>();
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

    IEnumerator checkSettingParam(GameObject player){
        yield return new WaitForSeconds(timeToloadSettings);

        PlayerStats ps = player.GetComponent<PlayerStats>();

        if(ps == null){
            Debug.LogError("Player doesn't have PLayerStats.cs!");
            StopAllCoroutines();
        }

        //Check enum from tile settings
        switch((int)tType){
            case 0: // free Win to that player
                ps.wins++;
                break;
            case 1: //Call to start a minigame
                StartCoroutine(mgm.StartMiniGame());
                break;
            case 2: //Give a buff to the player
                switch((int)bType){
                    case 0: //
                        ps.speed += buffAmount;
                        break;
                    case 1:
                        ps.jumpPower += buffAmount;
                        break;
                    case 2:
                        ps.damage += (int)buffAmount;
                        break;
                }
                break;
        }

        //Stop coroutine
        StopAllCoroutines();
    }

    bool checkPlayer(GameObject player){

        if(player != null && pm != null){

            for(int i = 0; i < pm.player.Length; i++){
                if(pm.player[i] == player){
                    Debug.Log("Found player in player manager!");
                    playerIndex = i;
                    return true;
                }
            }

            Debug.LogError("Player doesn't exist in player manager!");
            return false;

        }

        return false;
    }

    //local use only
    int playerIndex;

    void OnTriggerEnter2D(Collider2D collider){
        //Check if it's a player
        if(collider.gameObject.tag == "Player"){
            //sort player to spot
            if(checkPlayer(collider.gameObject)){
                PlayerControls pc = collider.gameObject.GetComponent<PlayerControls>();
                //depending on the index move that player to the corresponding spot inside the tile
                for(int i = 0; i < playerPositions.Length; i++){
                    if(i == playerIndex){
                        //Move player to that new position
                        pc.newTile = playerPositions[i];
                        pc.hasRan = false;
                    }
                }
            }

            //Do something
            StartCoroutine(checkSettingParam(collider.gameObject));
        }
    }
}

public enum tileType { freeWin, miniGame, buff}
public enum buffType {jumpBuff, speedBuff, damageBuff}