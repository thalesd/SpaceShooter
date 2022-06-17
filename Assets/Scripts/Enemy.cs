using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject enemyExplodingParticles;
    public AudioClip enemyExplodingAudio;

    public float speed = 1.5f;

    private void Update()
    {
        if(GameManager.instance.isGameRunning)
            transform.Translate(new Vector2(0, -1) * speed * Time.deltaTime);
    }

    public void Die()
    {
        Instantiate(enemyExplodingParticles, transform.position, Quaternion.identity);
        SFXAudioSource.Instance.PlayGameplayEffectOneShot((int)GameplaySFX.EnemyDestroyed);

        GameManager.instance.InstantiateDebree(transform.position);

        GameManager.instance.RemoveEnemyFromList(gameObject);

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
