using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Rigidbody2D rb2d;
    public float speed = 120f;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();

        rb2d.AddForce(transform.up * speed);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        var enemy = collider.gameObject.GetComponent<Enemy>();

        if (enemy != null)
        {
            enemy.Die();

            Destroy(gameObject);
        }
        else if (collider.gameObject.tag == "BulletCollector")
        {
            Destroy(gameObject);
        }        
    }
}
