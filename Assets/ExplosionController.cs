using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

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
