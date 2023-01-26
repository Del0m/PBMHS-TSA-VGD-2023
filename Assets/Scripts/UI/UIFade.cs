using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

public class UIFade : MonoBehaviour
{
    public Image canvas;
    public int fadeTime; // time to fade the UI
    public TextMeshProUGUI text; // optional fade

    public bool hasText; // to fade the text as well

    Color fadeColor;
    float time;
    public float waitTime;
    private void Start()
    {
        fadeColor = canvas.color; // to grab the color of the canvas

        canvas = this.GetComponent<Image>();
        if(hasText)
        {
            text = GetComponentInChildren<TextMeshProUGUI>();
        }
    }
    void FadeUI() // fades things over time
    {
        time += Time.deltaTime;
        if (time < waitTime) { return; } // prevent overruning of this script
        fadeColor.a -= (Time.deltaTime / fadeTime);
        canvas.color = new Color(canvas.color.r, canvas.color.g, canvas.color.b, fadeColor.a);

        if (fadeColor.a < 0) // allow buttons to be pressed
        {
            canvas.gameObject.SetActive(false);
        }
        print(canvas.color.a);
        try
        {
            if(hasText)
            {
                text.color = fadeColor;
            }
        }
        catch (System.Exception)
        {
        }
    }
    void Update()
    {
        FadeUI();
    }
}
