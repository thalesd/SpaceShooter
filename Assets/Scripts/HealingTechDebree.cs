using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingTechDebree : TechDebree
{
    [SerializeField]
    private int healingValue = 10;

    //default behavior
    public override void Collect()
    {
        GameManager.instance.playerReference.Heal(healingValue);

        Destroy(gameObject);
    }
}
