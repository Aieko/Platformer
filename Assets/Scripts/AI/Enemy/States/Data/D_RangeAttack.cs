using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "NewRangeAttackStateData", menuName = "Data/State Data/Range Attack State")]
public class D_RangeAttack : ScriptableObject
{
    public float projectileDamage = 10f;
    public float projectileSpeed =12f;
    public float projectileTravelDistance;
    public float launchAngle;

    

    public GameObject projectile;

    public LayerMask whatIsPlayer;
}
