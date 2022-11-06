using UnityEngine;

public class PointCheck : MonoBehaviour
{
    public Vector3 pos { get; private set; }

    public bool isOccupied { get; private set; } = false;

    // Start is called before the first frame update
    void Awake()
    {
        pos = this.transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        //Check if an entity is on the point
        if(other.gameObject.layer == LayerMask.NameToLayer("Entity")){
            isOccupied = true;

            //When it is a entity with this component check its turn variable and deduce it by an amount
            PlayerMovement pm = other.GetComponent<PlayerMovement>();
            if(pm != null)
            {
                if(pm.playerTurn != 0)
                {
                    //deduce a value from the player turn var and set this obj's position
                    pm.deduceTurn(1, pos);
                    //debug
                    print("Deduce a turn from the player");
                }
            }
            //debug
            print("There is an entity at " + this.name);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //Check if an entity left the point
        if (other.gameObject.layer == LayerMask.NameToLayer("Entity"))
        {
            isOccupied = false;
            //debug
            print("Entity left me");
        }
        else
        {
            isOccupied = false;
        }
    }
}
