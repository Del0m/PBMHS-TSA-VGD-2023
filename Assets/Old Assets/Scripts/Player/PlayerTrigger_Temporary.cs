using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTrigger_Temporary : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameManager_Temporary manager = GameManager_Temporary.instance.GetComponent<GameManager_Temporary>();
        if (collision.gameObject.tag == "EndLevel")
        {
            print("Reached end of level!");
            manager.EndOfGame();
        }
        else if (collision.gameObject.tag == "DeathPit")
        {
            print(gameObject.name + " has fallen into the abyss");
            manager.restartLevel();
        }
    }
}
