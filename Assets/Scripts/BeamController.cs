using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamController : MonoBehaviour
{
    private PlayerController player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        gameObject.GetComponent<Animator>().SetBool("IsFiring", player.isFiringLaserBeam);
    }

    public void DestroySelf()
    {
        Destroy(gameObject, 2f);
    }
}
