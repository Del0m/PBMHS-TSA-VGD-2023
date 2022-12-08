using UnityEngine;

public class FerryMan : MonoBehaviour
{
    [Header("Parameters")]
    public GameObject transitionPoint;
    public float speed;
    public float minDistance;

    bool isMove;

    //for making player work good on boat.
    [SerializeField]
    private GameObject wallColliders;
    private GameObject player;


    private Rigidbody2D rb;
    private Animator anim;

    private void Start()
    {
        anim = this.GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        player = PlayerManagerOld.instance.playerInstance;
    }

    void Update()
    {
        Move();
        if(anim == null) 
        {
            anim = this.GetComponent<Animator>();
        }
    }

    void Move()
    {
        if(isMove == true && transitionPoint != null && rb != null)
        {
            anim.SetBool("rowingactive", true);
            if(Vector2.Distance(this.transform.position, transitionPoint.transform.position) > minDistance)
            {
                rb.velocity = new Vector2(speed, rb.velocity.y);
                wallColliders.SetActive(true);

            }
            else if(Vector2.Distance(this.transform.position, transitionPoint.transform.position) < minDistance)
            {
                isMove = false;
                anim.SetBool("rowingactive", false);
                wallColliders.SetActive(false);
            }
        }       
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isMove = true;
        }
        else
        {
            // nothing
        }
    }
}
