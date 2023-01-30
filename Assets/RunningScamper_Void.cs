using UnityEngine;

public class RunningScamper_Void : MonoBehaviour
{
    [Header("Params")]
    public GameObject cageObj;
    public GameObject cam;
    public PhysicsMaterial2D pm;
    private void Start()
    {
        //cageObj = GameObject.Find("Cage");
        cam = GameObject.FindGameObjectWithTag("MainCamera");
        if(cageObj == null)
        {
            Debug.LogError("Couldn't find Cage!");
            return;
        }
        else
        {
            Debug.Log("Found the cage!");
        }
    }
    

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player" && pm != null)
        {
            PlayerMovement player = collision.gameObject.GetComponent<PlayerMovement>();
            if(player != null)
            {
                //turn off camera
                CamViewMObjs cm = cam.GetComponent<CamViewMObjs>();
                if(cm != null)
                {
                    //remove this player from list
                    cm.targets.Remove(collision.gameObject.transform);

                }

                BoxCollider2D pBox = collision.gameObject.GetComponent<BoxCollider2D>();
                Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
                if(pBox != null && rb != null){
                    //Replace physics material
                    pBox.sharedMaterial = pm;
                    rb.sharedMaterial = pm;
                }

                //Teleport
                collision.gameObject.transform.position = new Vector3(cageObj.transform.position.x, cageObj.transform.position.y, cageObj.transform.position.z);
                //Set player as child of location
                collision.gameObject.transform.parent = cageObj.transform;
                
                //Modify player
                player.setStaticMovement(false);
                player.GameSwitch(true);
                player.setFreeMovement(true);
            }else if(pm == null){
                Debug.Log("No Physic Material 2D set to add to players");
                return;
            }
            Debug.Log("Player got sent to the cage!"); // Insert kinky thought here
        }
    }
}
