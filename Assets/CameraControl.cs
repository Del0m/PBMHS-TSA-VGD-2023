using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    private Camera cam; // camera that will move
    private GameObject self; // self just for code brevity.
    private void Awake() // set camera to itself
    {
        cam = this.gameObject.GetComponent<Camera>();
        self = gameObject;
    }

    // Update is called once per frame; moves camera to intended location
    void Update()
    {
        
    }
}
