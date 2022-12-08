using UnityEngine;

public class PlayerFeet : MonoBehaviour
{
    // Start is called before the first frame update
    public bool canJump { get; private set; }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Environment"))
        {
            canJump = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Environment"))
        {
            canJump = true;
        }
    }

    /*private void LateUpdate()
    {
        Invoke("cancelBool", 2.5f);
    }

    void cancelBool()
    {
        canJump = true;
    }
    */
}
