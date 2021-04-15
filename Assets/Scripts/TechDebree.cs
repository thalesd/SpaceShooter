using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TechDebree : MonoBehaviour, ICollectible
{
    public DebreeEnum type;

    public float speed = 2.5f;

    private void Update()
    {
        Vector2 downSpeed = new Vector2(Random.Range(-1f, 1f), 0);

        transform.Translate(downSpeed * speed * Time.deltaTime);
    }

    public virtual void Collect()
    {
        throw new System.NotImplementedException();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Teleporter")
        {
            Destroy(gameObject);
        }
    }
}
