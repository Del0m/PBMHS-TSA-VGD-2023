using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [HideInInspector]
    public int TotalEntityCount; // Set the maximum amount of enemies in an area

    public GameObject[] Entities;

    [Header("Amount of Objects/Entities")]
    public int MinEntityCount;
    public int MaxEntityCount;

    [Header("Spawn Area Size")]
    public float xPosMin = -10f;
    public float xPosMax = 10f;
    public float yPosMin = -10f;
    public float yPosMax = 10f;

    int randomEntity;
    float xPos;
    float yPos;
    int EntityCount;

    [Header("Delay Values")]
    public float initialDelay; // Can be set to zero

    public bool Stop = false;

    void Start()
    {
        TotalEntityCount = Random.Range(MinEntityCount, MaxEntityCount);
    }

    void LateUpdate()
    {
        if (Stop == false)
        {
            randomEntity = Random.Range(0, Entities.Length);
            StartCoroutine(EnemySpawn());
        }
        else
        {
            StopAllCoroutines();
        }
    }

    void FixedUpdate()
    {
        if (Stop == false)
        {
            ErrorCheck();
        }
    }

    void ErrorCheck()
    {
        if (TotalEntityCount < EntityCount)
        {
            Stop = true;
            StopAllCoroutines();
            this.enabled = false;
            Debug.LogWarning(gameObject.name + " Has spawned the exceeded amount of entities & was forced to stop");
            return;
        }
    }

    IEnumerator EnemySpawn()
    {
        while (!Stop)
        {
            yield return new WaitForSeconds(initialDelay);
            for (int i = 0; i <= TotalEntityCount; i++)
            {
                WaitForSeconds wait = new WaitForSeconds(4f);
                xPos = Random.Range(xPosMin, xPosMax);
                yPos = Random.Range(yPosMin, yPosMax);
                Instantiate(Entities[randomEntity], new Vector3(xPos, yPos, 0) + gameObject.transform.position, Quaternion.identity);
                ++EntityCount;
                if (EntityCount >= TotalEntityCount)
                {
                    Stop = true;
                }
                yield return wait;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(gameObject.transform.position, new Vector3(xPosMax, yPosMax, 0));
    }
}