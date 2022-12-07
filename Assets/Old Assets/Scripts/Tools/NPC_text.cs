 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_text : MonoBehaviour
{
    private bool isNear;
    private bool interaction = false;
    private bool checkBoxes = false;
    private int textSelection = -1;
    private GameObject player;
    [Header("Canvas parameters")]
    public GameObject canvasObject;
    public GameObject inputCanvas;
    public bool notloopable;
    private List<GameObject> textBoxes ;

    public float radius = 3f;

    private bool inputEnable = true;

    // Start is called before the first frame update
    void Start()
    {
        AddChildren();
    }

    void AddChildren()
    {
        //Adds all text boxes to the list
        textBoxes = new List<GameObject>();
        foreach (Transform tran in canvasObject.transform)
        {
            textBoxes.Add(tran.gameObject);
        }
    }

    void FindPlayer()
    {
        if (PlayerManagerOld.instance.playerInstance != null)
        {
            player = PlayerManagerOld.instance.playerInstance;
        }
        else
        {
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(player == null)
        {
            FindPlayer();
            if(inputCanvas == null)
            {
                Debug.LogError(this.name + " couldn't find player nor input canvas found!");
                return;
            }
        }
        else
        {
            checkForDistance();
            if (notloopable == true)
            {
                if (textSelection >= textBoxes.Count)
                {
                    if (Input.GetKey(KeyCode.E))
                    {
                        //Deactivate input canvas and script
                        inputCanvas.SetActive(false);
                        inputEnable = false;
                    }
                }
            }
            else
            {
                if(textSelection < textBoxes.Count)
                {
                    inputEnable = true;
                }
                else
                {
                    inputCanvas.SetActive(false);
                    textSelection = -1;
                }
            }
        }   
    }

    void checkForDistance()
    {
        if (inputEnable != false)
        {
            float distance = Vector3.Distance(player.transform.position, transform.position);
            if (distance < radius)
            {
                isNear = true;
                if(textSelection == -1)
                {
                    inputCanvas.SetActive(true);
                }
            }
            else if (distance > radius)
            {
                inputCanvas.SetActive(false);
                isNear = false;
                checkBoxes = true;
            }
            InteractInput();
        }
        else
        {
            for (int i = 0; i < textBoxes.Count;)
            {
                textSelection = -1;
                if (i == textBoxes.Count)
                {
                    checkBoxes = false;
                    if(notloopable == false)
                    {
                        inputEnable = true;
                    }
                }
                textBoxes[i].SetActive(false);
                i++;
            }
        }
    }

    void CheckForSelection(int i)
    {
        if(i > -1)
        {
            textBoxes[i].SetActive(true);
            if(i > 0)
            {
                textBoxes[i - 1].SetActive(false);
            }
            //print(textBoxes[i].gameObject.name + " has been selected");
            StartCoroutine(waitForNext());
        }
    }

    void InteractInput()
    {
        if(textSelection < textBoxes.Count)
        {
            CheckForSelection(textSelection);
        }
        if(isNear == true && inputEnable == true) //When in radius, start dialogue
        {
            if (Input.GetKeyDown(KeyCode.E) && interaction == false)
            {
                textSelection++;
                print("Continuing text");
                interaction = true;
            }
        }
        else if(isNear == false && checkBoxes == true && notloopable == false) //Exit out of radius, reset text selection
        {
            textSelection = -1;
            for(int i = 0; i < textBoxes.Count;)
            {
                if(i == textBoxes.Count)
                {
                    checkBoxes = false;
                    inputEnable = true;
                }
                textBoxes[i].SetActive(false);
                i++;            
            }
        }
    }

    //[]Make this into a cooldown for input
    IEnumerator waitForNext()
    {
        yield return new WaitForSeconds(0.4f);
        interaction = false;
        StopCoroutine(waitForNext());
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
