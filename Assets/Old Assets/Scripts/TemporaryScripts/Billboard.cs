using UnityEngine;

public class Billboard : MonoBehaviour
{
    public GameObject cam;

    private void Start()
    {
        GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
        cam = camera;
    }

    void FixedUpdate()
    {
        transform.LookAt(transform.position + cam.transform.forward);
    }
}
