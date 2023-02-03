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

    IEnumerator checkSettingParam(GameObject player){
        yield return new WaitForSeconds(timeToloadSettings);

        PlayerStats ps = player.GetComponent<PlayerStats>();

        if(ps == null){
            Debug.LogError("Player doesn't have PLayerStats.cs!");
            StopAllCoroutines();
        }

        if(buffAmount <= 0)
        {
            Debug.Log(this.name + " doesn't have a buff to add!");
            StopAllCoroutines();
        }

        //Check enum from tile settings
        switch((int)tType){
            case 0: // free Win to that player
                ps.wins++;
                break;
            case 1: //Give a buff to the player
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

        Debug.Log("Tile Choise is [" + tType + "," + bType + "]");

        //Stop coroutine
        StopAllCoroutines();
    }

    bool checkPlayer(GameObject player){

        if(pm != null && player != null){

            playerIndex = -1;

            for(int i = 0; i < pm.player.Length; i++){
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

    IEnumerator waitToBuff(GameObject p){
        yield return new WaitForSeconds(2f);

        //Give buff and stop
        if(player.hasReachedDestination){ //has reached should be true if player stays on the tile
            StartCoroutine(checkSettingParam(p));
        }
    }
    void OnTriggerEnter2D(Collider2D collider){
        //Check if it's a player
        if(collider.gameObject.tag == "Player"){
            Debug.Log("FOUND PLAYER");
            //sort player to spot
            if(checkPlayer(collider.gameObject) == true){
                player = collider.gameObject.GetComponent<PlayerControls>();
                //depending on the index move that player to the corresponding spot inside the tile
                for(int i = 0; i < playerPositions.Length; i++){
                    if(i == playerIndex){
                        //Move player to that new position
                        Debug.Log("Moving to tile");
                        player.newTile = playerPositions[i];
                        player.hasRan = false;
                    }
                }
            }

            //Start a delay to then give player buff
            StartCoroutine(waitToBuff(collider.gameObject));
            
        }
    }
}

public enum tileType { freeWin, buff}
public enum buffType {jumpBuff, speedBuff, damageBuff}