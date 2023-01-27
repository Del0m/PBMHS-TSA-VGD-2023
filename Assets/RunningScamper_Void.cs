using UnityEngine;

public class RunningScamper_Void : MonoBehaviour
{
    public GameObject cageObj;
    public GameObject cam;
    
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
        if(collision.gameObject.tag == "Player")
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

                //Turn off static dir
                player.setStaticMovement(false);
                player.GameSwitch(true);
                collision.gameObject.transform.position = new Vector3(cageObj.transform.position.x, cageObj.transform.position.y, cageObj.transform.position.z);
            }
            Debug.Log("Player got sent to the cage!"); // Insert kinky thought here
        }
    }
}
