using UnityEngine;

public class Scamper_GridMap : MonoBehaviour
{
    [Header("Params")]
    public float timer = 25f;

    public float speed = 2f;

    //Flip the map randomly on start
    private void Start()
    {
        int i = Random.Range(0, 2);
        if (i == 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
            Debug.Log("Flipped");
        } //Flip when i is zero
    }

    // Update is called once per frame
    void Update()
    {
        Invoke("goDown", timer);
    }

    void goDown(){
        transform.Translate(Vector2.down * speed * Time.deltaTime);
        Destroy(this.gameObject, 10f);
    }
}
