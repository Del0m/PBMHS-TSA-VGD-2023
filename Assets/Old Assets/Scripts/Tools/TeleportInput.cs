using UnityEngine;

public class TeleportInput : MonoBehaviour
{
    [Header("Visual Input")]
    public GameObject inputCanvas;

    [Header("Location to teleport to")]
    public GameObject TeleportLocation;

    [Header("Interaction distance")]
    public float radius;

    private bool hasInteracted;
    private GameObject player;
    private GameObject playerCamera;

    private void Start()
    {
        inputCanvas.SetActive(false);
    }

    void FindInstances()
    {
        if(PlayerManagerOld.instance.playerInstance != null || PlayerManagerOld.instance.cameraInstance != null)
        {
            player = PlayerManagerOld.instance.playerInstance;
            playerCamera = PlayerManagerOld.instance.cameraInstance;
        }
        else
        {
            PlayerManagerOld.instance.setInstances();
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(player == null || playerCamera == null)
        {
            FindInstances();
            if(TeleportLocation == null)
            {
                Debug.LogError("No teleport location specified!");
                return;
            }
        }
        else if(player != null && TeleportLocation != null)
        {
            CheckForDistance();
            CheckForInput();
        }
    }

    void CheckForInput()
    {
        if(hasInteracted == true && Input.GetKeyDown(KeyCode.E))
        {
            // Teleport player to the desired point
            player.transform.position = TeleportLocation.transform.position;

            playerCamera.transform.position = TeleportLocation.transform.position;

            //Disables input if the 'CheckForDistance' doesn't update
            hasInteracted = false;
        }
    }

    void CheckForDistance()
    {
        float distance = Vector3.Distance(player.transform.position, transform.position);
        if (distance < radius)
        {
            hasInteracted = true;
            inputCanvas.SetActive(true);
        }
        else
        {
            hasInteracted = false;
            inputCanvas.SetActive(false);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
