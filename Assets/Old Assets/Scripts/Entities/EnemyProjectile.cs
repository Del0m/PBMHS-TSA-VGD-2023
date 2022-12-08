using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyProjectile : MonoBehaviour
{
    [HideInInspector]
    public float projectileDamage;

    [Header("Properties")]
    public float speed = 10f;

    private Vector3 shootDirection;

    private void Awake()
    {
        Destroy(gameObject, 4f);
    }

    public void setup(Vector3 shootDir)
    {
        this.shootDirection = shootDir;
        transform.eulerAngles = new Vector3(0, 0, GetAngleFromVectorFloat(shootDir));
    }

    static float GetAngleFromVectorFloat(Vector3 dir)
    {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;

        return n;
    }

    private void Update()
    {
        transform.position += shootDirection * speed * Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
        if (collision.gameObject.tag == "Player")
        {
            EntityStats health = collision.gameObject.GetComponent<EntityStats>();
            if (health != null)
            {
                health.TakeDamage(projectileDamage);
                Destroy(gameObject);
            }
        }
        else if(collision.gameObject.layer != 11)
        {
            Destroy(gameObject);
        }

    }
}
