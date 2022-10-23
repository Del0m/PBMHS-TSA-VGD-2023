using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Randomizer : MonoBehaviour
{
    
    public int DiceRoll(int low, int high)
    {
        //Random number gen giving result back to user
        int result = Random.Range(low, high);
        return result;
    }
  
    private void Start()
    {
        
        print(DiceRoll(1, 6));
    }
}
