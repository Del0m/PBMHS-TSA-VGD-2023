using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Card : MonoBehaviour
{
    //card calling variables that are important to game
    public int value;
    public TextMeshProUGUI text;

    //grabbing game mother
    public MathDash minigame;
    private void Update()
    {
        Debug.Log(this.gameObject.activeInHierarchy);
    }
    private void Start()
    {

        if(text != null)
        {
            text.text = value.ToString();
        }
        else
        {
            text = GetComponentInChildren<TextMeshProUGUI>(); // grabbing text to modify.
        }
        if(minigame == null)
        {
            minigame = GameObject.FindGameObjectWithTag("Minigame").GetComponent<MathDash>();
        }    
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Debug.Log("Player has entered.");
            //check to see if they're acting.
            if (collision.GetComponent<PlayerMovement>().acting == true)
            {
                var clicker = collision.gameObject;
                print(clicker);
                Debug.Log("Checking!");
                //checking answer, and deleting card if its correct.
                minigame.CheckAnswer(clicker, value); // collision = player, value = card value

                Destroy(this.gameObject, 2.5f); // destroy object after checking for the answer.
            }
        }
    }
}
