using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawn : MonoBehaviour
{
    // Start is called before the first frame update
    static GameObject ogma;
    void Start()
    {
        ogma = Instantiate(Resources.Load("Prefabs/Mini-Games/Math Dash", typeof(GameObject))) as GameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
