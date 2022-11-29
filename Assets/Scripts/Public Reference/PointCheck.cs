using UnityEngine;

public class PointCheck : MonoBehaviour
{
    public Vector3 pos { get; private set; }

    public bool isOccupied { get; private set; } = false;

    public int correspondingPlayerIndex = -1;

    public bool hasSeenPlayer = false;

    Renderer mesh;

    // Start is called before the first frame update
    void Awake()
    {
        pos = this.transform.position;
        mesh = GetComponent<Renderer>();
        changeToPlayerColor();
    }

    void changeToPlayerColor()
    {
        switch (correspondingPlayerIndex)
        {
            case -1:
                mesh.material.color = Color.white;
                break;
            case 0:
                mesh.material.color = Color.blue;
                break;
            case 1:
                mesh.material.color = Color.red;
                break;
            case 2:
                mesh.material.color = Color.green;
                break;
            case 3:
                mesh.material.color = Color.yellow;
                break;
        }
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

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Entity"))
        {
            hasSeenPlayer = false;
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
