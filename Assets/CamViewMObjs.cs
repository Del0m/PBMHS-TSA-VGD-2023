using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamViewMObjs : MonoBehaviour
{
    [Header("Cam View params")]
    public List<Transform> targets;
    public float delay = 10f;
    public Vector3 offSet;

    private GameHandler gameH;

    private void Start()
    {
        //Get players and define as targets
        GameObject gh = GameObject.FindGameObjectWithTag("minigame");
        gameH = gh.GetComponent<GameHandler>();
        
        if(gameH != null)
        {
            StartCoroutine(findPlayers());
        }
    }

    IEnumerator findPlayers()
    {
        yield return new WaitForSeconds(delay);
        if(gameH.player.Length> 0 )
        {
            for(int i = 0; i < gameH.player.Length; i++)
            {
                targets.Add(gameH.player[i].transform);
            }
            Debug.Log("finished adding players");
        }
    }

    private void LateUpdate()
    {
        if(targets.Count == 0)
        {
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
            return.targets[0].position;
        }

        var bounds = new Bounds(targets[0].position, Vector3.zero);
        for(int i = 0; i < targets.Count; i++)
        {
            bounds.Encapsulate(targets[i].position);
        }

        return bounds.center;
    }
}
