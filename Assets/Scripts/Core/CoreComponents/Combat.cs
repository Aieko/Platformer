using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : CoreComponent, IDamageable
{
    public void Damage(AttackDetails attackDetails)
    {
        Debug.Log(core.transform.parent.name + " Damaged!");
    }


}
