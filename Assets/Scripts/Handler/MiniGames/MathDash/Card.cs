using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(EntityStats))]
public class Card : MonoBehaviour
{
    public int value;
    public TextMeshProUGUI text;

    //grabbing game mother
    public MathDash minigame;

    //rigidbody to send in direction
    public Rigidbody2D rb;

    // stats to kill card and award to clicker
    public EntityStats stat;

    bool onGoing; // to prevent multiple runs of CheckAnswer()

    // for modifying health bar
    public GameObject healthUI; 
    private void Update()
    {
        if(stat.killer != null)
        {
            StartCoroutine(CheckAnswer(stat.killer));
        }
        HealthBar();
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
    void HealthBar() // modify health bar on answer card
    {
        var healthPercent = stat.health * .1;
        healthUI.transform.localScale = new Vector3(((float)healthPercent), 1, 1);
    }

    Vector2 ChooseVectorDirection() // calculates random vector to shoot card at
    {
        var angle = Random.Range(0, 360); // random angle 0 - 360
        var radAngle = angle * Mathf.Deg2Rad; // turn to radians to calculate vector

        var randVect = new Vector2(Mathf.Cos(radAngle), Mathf.Sin(radAngle)); //some weird trig shit, dunno what it means.
        return randVect;
    }
    IEnumerator CheckAnswer(GameObject clicker)
    {
        // check to see if already running
        if(onGoing == true) { yield break; } 
        onGoing = true;

        //checking answer, and deleting card if its correct.
        if(minigame.CheckAnswer(clicker, value) == true) // clicker = player, value = card value
        {
            text.text = "Right!"; // tell user 
        }
        else
        {
            text.text = "Wrong!"; // tell user
        }
        yield break;
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
