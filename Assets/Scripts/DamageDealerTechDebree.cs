using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealerTechDebree : TechDebree
{
    private int damageValue = 10;

    //default behavior
    public override void Collect()
    {
        GameManager.instance.playerReference.Damage(damageValue);

        Destroy(gameObject);
    }
}
