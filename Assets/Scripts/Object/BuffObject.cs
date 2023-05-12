using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Objects/Buff")]
public class BuffObject : ScriptableObject
{
    [Header("Stats")]
    public float multiplier; // how much the buff will affect the user
    public float time; // how long the buff will last

    [Header("Cosmetic")] // how it will show in the UI
    public Color color;
}
