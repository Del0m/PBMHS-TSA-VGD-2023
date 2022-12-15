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
    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.collider.tag == "Player")
        {
            //check to see if they're acting.
            if(collision.collider.GetComponent<PlayerMovement>().acting == true)
            {
                //checking answer, and deleting card if its correct.
                minigame.CheckAnswer(collision.collider.gameObject, value); // collision = player, value = card value
            }
        }
    }
}
