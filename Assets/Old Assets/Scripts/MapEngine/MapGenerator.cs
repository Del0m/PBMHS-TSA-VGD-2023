using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public Transform[] gridMapsSpawnPoints;

    public GameObject[] GridMapPrefabs; 

    private int gridMapSelection = 0;
    private int gridMapPrefabSelection = 0;

    private int currentGridMaps;
    private int totalGridMaps;

    bool stop;

    void Awake()
    {
        //Checks if there are points assigned to spawn in grid maps
        ErrorCheck();
        //Sets the total amount of grid maps that are going to be spawned to the lenght of the assigned spawn points
        totalGridMaps = gridMapsSpawnPoints.Length;
    }

    void ErrorCheck()
    {
        if(gridMapsSpawnPoints == null || gridMapsSpawnPoints.Length <= 0)
        {
            Debug.LogError("No grid spawn points assigned!");
            stop = true;
            return;
        }
    }


    // Update is called once per frame
    void Update()
    {
        gridMapPrefabSelection = Random.Range(0, GridMapPrefabs.Length);
        CheckMap();
        StartCoroutine(SpawnInMaps());
    }

    void CheckMap()
    {
        //Checks if it has spawned all the needed grid maps on the spawnpoints
        currentGridMaps = gridMapSelection;
        if(currentGridMaps == totalGridMaps)
        {
            stop = true;
            StopAllCoroutines();
        }
    }

    IEnumerator SpawnInMaps()
    {
        while (!stop)
        {
            for(int i = 0; i <= gridMapsSpawnPoints.Length; i++)
            {
                WaitForSeconds wait = new WaitForSeconds(5f);

                Instantiate(GridMapPrefabs[gridMapPrefabSelection], gridMapsSpawnPoints[gridMapSelection].position, gridMapsSpawnPoints[gridMapSelection].rotation, gridMapsSpawnPoints[gridMapSelection]);
                gridMapSelection++;
                if(gridMapSelection == gridMapsSpawnPoints.Length)
                {
                    stop = true;
                }

                yield return wait;
            }
        }
    }
}
