using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX.Utility;

public class Enemy : MonoBehaviour
{
    public GameObject enemyExplodingParticles;

    public float speed = 1.5f;

    private void Update()
    {
        transform.Translate(new Vector2(0, -1) * speed * Time.deltaTime);
    }

    public void Die()
    {
        //instantiate explode anim/particle system
        Instantiate(enemyExplodingParticles);

        GameManager.instance.InstantiateDebree(transform.position);

        GameManager.instance.RemoveEnemyFromList(gameObject);

        //destroy game object 
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Teleporter")
        {
            transform.position = new Vector2(transform.position.x, 6);
        }
    }
}
