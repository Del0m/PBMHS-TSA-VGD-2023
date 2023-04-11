using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIFade : MonoBehaviour
{
    public Image canvas;
    public int fadeTime; // time to fade the UI
    public TextMeshProUGUI[] text; // optional fade

    public bool hasText; // to fade the text as well

    [Header("Pause Variables")]
    public PlayerManager plrManage;
    private List<GameObject> player;
    public bool pauseGame; // to pause the game for the UI before fading
    private bool hasActed; // once player clicks, fade function starts

    Color fadeColor;
    float time;
    public float waitTime;
    private void Start()
    {
        fadeColor = canvas.color; // to grab the color of the canvas

        canvas = this.GetComponent<Image>();
        if(hasText)
        {
        }
        if(pauseGame)
        {
            plrManage = GameObject.FindGameObjectWithTag("Player Manager").GetComponent<PlayerManager>();
            player = plrManage.player; // grab player array from the ui fade.
        }
    }
    void FadeUI() // fades things over time
    {
        if(!hasActed && pauseGame) { return;  } // prevent running when its supposed to pause normally.

        time += Time.deltaTime;
        if (time < waitTime) { return; } // prevent overruning of this script
        fadeColor.a -= (Time.deltaTime / fadeTime);
        canvas.color = new Color(canvas.color.r, canvas.color.g, canvas.color.b, fadeColor.a);

        if (fadeColor.a < 0) // allow buttons to be pressed
        {
            canvas.gameObject.SetActive(false);
        }
        try
        {
            if(hasText)
            {
                for(int i = 0; i < text.Length; i++)
                {
                    text[i].color = fadeColor;

                }
            }
        }
        catch (System.Exception)
        {
        }
    }
    void Update()
    {
        FadeUI();

        if(pauseGame && player.Count != 0)
        {
            if(player[0].GetComponent<PlayerMovement>().acting == true || player[0].GetComponent<PlayerMovement>().canJump == true)
            {
                hasActed = true;
            }
        }
    }
}
