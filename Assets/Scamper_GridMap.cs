using UnityEngine;

public class Scamper_GridMap : MonoBehaviour
{
    [Header("Params")]
    public float speed = 2f;
    public float timer = 25f;

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
