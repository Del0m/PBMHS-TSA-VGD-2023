// armin delmo healthbar.cs; this modifies the healthbar's progress meter.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healthbar : MonoBehaviour
{
    public RectTransform healthbar;
    public void ModifyProgress(float current, float max) // change health bar length to represent progress
    {
        Debug.Log(current / max * 100);
        var progress = (current / max) * 100;
        healthbar.sizeDelta = new Vector2(progress, healthbar.sizeDelta.y);
    }
}
