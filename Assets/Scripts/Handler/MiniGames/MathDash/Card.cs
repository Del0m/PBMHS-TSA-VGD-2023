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

    //rigidbody to send in direction
    public Rigidbody2D rb;

    private bool hasClicked; // to prevent multiple runs of game
    private void Update()
    {
        //having card move
        //use rigidbody addforce in one direction super fast and just let it bounce over time.



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
        //making angle it will shoot at
        StartCoroutine(SwitchDirection(10,10));
    }

    Vector2 ChooseVectorDirection() // calculates random vector to shoot card at
    {
        var angle = Random.Range(0, 360); // random angle 0 - 360
        var radAngle = angle * Mathf.Deg2Rad; // turn to radians to calculate vector

        var randVect = new Vector2(Mathf.Cos(radAngle), Mathf.Sin(radAngle)); //some weird trig shit, dunno what it means.
        return randVect;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player" && hasClicked == false) // check statement to not repeat runs
        {
            Debug.Log("Player has entered.");
            //check to see if they're acting.
            if (collision.GetComponent<PlayerMovement>().acting == true && hasClicked == false)
            {
                hasClicked = true;
                var clicker = collision.gameObject;
                print(clicker);
                Debug.Log("Checking!");
                //checking answer, and deleting card if its correct.
                minigame.CheckAnswer(clicker, value); // collision = player, value = card value

                Destroy(this.transform.parent.gameObject, 1f); // destroy whole object after checking for the answer.
            }
        }
    }
    IEnumerator SwitchDirection(int second, int i)
    {
        var rand = Random.Range(-3f, 4f);
        for(int j = 0; j < i; j++)
        {
            rb.velocity = ChooseVectorDirection() * 5; // randomly chooses direction, bouncy material will keep it from losing traction.
            yield return new WaitForSeconds(second+rand);
        }

    }
}
