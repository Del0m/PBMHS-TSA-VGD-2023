using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Randomizer : MonoBehaviour
{
    public Text diceUI;

    public int DiceRoll(int low, int high)
    {
        //Random number gen giving result back to user
        int result = Random.Range(low, high);
        return result;
    }
  
    private void Start()
    {
	int finalResult = DiceRoll(1, 6);
        diceUI.text = finalResult.ToString();
        print(finalResult);
    }
}
