using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamViewMObjs : MonoBehaviour
{
    [Header("Cam View params")]
    public List<Transform> targets;
    public float delay = 10f;
    public Vector3 offSet = new Vector3(0, 0, -25);

    private RunningScamper gameH;
    private PlayerManager pm;

    public List<GameObject> forgottenTargets; //used to not track a player again

    bool isBoard = true;

    private void Start()
    {
        findPlayersMinigame();
    }

    public void setCamToMinigame(){
        isBoard = false;
        //Get players and define as targets
        GameObject gh = GameObject.FindGameObjectWithTag("Minigame");
        gameH = gh.GetComponent<RunningScamper>();
    }

    public void disableCamToMinigame(){
        isBoard = true;
        pm = GameObject.FindGameObjectWithTag("Player Manager").GetComponent<PlayerManager>();
        gameH = null;
        targets.Clear();
    }

    bool findPlayersMinigame(){
        if(gameH != null){
            if(gameH.player.Length > 0 && gameH.allowCameraFollow == true){
                for(int i = 0; i < gameH.player.Length; i++)
                {
                    //Check if player is not out of the game
                    if(forgottenTargets.Count > 0){
                        if(forgottenTargets[i] != gameH.player[i]){
                            targets.Add(gameH.player[i].transform);
                        }
                    }else{
                        targets.Add(gameH.player[i].transform);
                    }
                }
                Debug.Log("finished adding players");
                return true;
            }else if(gameH.allowCameraFollow == false){
                targets.Clear();
                return false;
            }
            return false;
        }
        return false;
    }

    bool findPlayers(){

        if(pm != null){
            if(pm.player.Length > 0){
                for(int i = 0; i < pm.player.Length; i++){
                    //Add players from pm to be as targets
                    targets.Add(pm.player[i].transform);
                }

                return true;
            }

            Debug.LogError("No player found in player manager!");
            return false;
        }

        return false;
    }


    private void LateUpdate()
    {
        if(targets.Count == 0)
        {
            if(isBoard)
                findPlayers();
            else
                findPlayersMinigame();
            return;
        }

        
        Vector3 centerPoint = GetCenterPoint();

        Vector3 newPosition = centerPoint + offSet;

        transform.position = newPosition;
        
    }

    Vector3 GetCenterPoint()
    {
        if(targets.Count == 1)
        {
            return targets[0].position;
        }

        var bounds = new Bounds(targets[0].position, Vector3.zero);
        for(int i = 0; i < targets.Count; i++)
        {
            bounds.Encapsulate(targets[i].position);
        }

        return bounds.center;
    }
}
