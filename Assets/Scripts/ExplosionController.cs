using UnityEngine;

public class ExplosionController : MonoBehaviour
{
    public float destroyTime = 2.5f;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, destroyTime);
    }
    
    public void OnStop()
    {
        Destroy(gameObject);
    }
}
