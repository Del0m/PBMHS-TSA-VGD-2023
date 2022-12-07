using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSplitGenerator : MonoBehaviour
{
    public GameObject[] leftGridMaps;
    public GameObject[] rightGridMaps;

    public Transform[] gridMapsSpawnPoints;

    int spawnPointSelection;
    int currentLeftGridMapSelection;
    int currentRightGridMapSelection;

    int totalGridMap = 0;

    private void Awake()
    {
        ErrorCheck();
        currentLeftGridMapSelection = Random.Range(0, leftGridMaps.Length);
        currentRightGridMapSelection = Random.Range(0, rightGridMaps.Length);
    }

    private void Update()
    {
        spawnPointSelection = Random.Range(0, gridMapsSpawnPoints.Length);
        ErrorCheck();
        Invoke("SpawnInMaps", 0.5f);
    }

    void SpawnInMaps()
    {
        if (totalGridMap == 0)
        {
            if (spawnPointSelection == 0) //Left Spawn point
            {
                Instantiate(leftGridMaps[currentLeftGridMapSelection], gridMapsSpawnPoints[spawnPointSelection].position, gridMapsSpawnPoints[spawnPointSelection].rotation, gridMapsSpawnPoints[spawnPointSelection]);
            }
            else if (spawnPointSelection == 1) //Right Spawn Point
            {
                Instantiate(rightGridMaps[currentRightGridMapSelection], gridMapsSpawnPoints[spawnPointSelection].position, gridMapsSpawnPoints[spawnPointSelection].rotation, gridMapsSpawnPoints[spawnPointSelection]);
            }
            totalGridMap++;
        }
    }

    void ErrorCheck()
    {
        if (gridMapsSpawnPoints == null || gridMapsSpawnPoints.Length <= 0)
        {
            Debug.LogError("No grid spawn points assigned!");
            return;
        }
    }
}
