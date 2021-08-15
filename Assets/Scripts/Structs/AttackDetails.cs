using UnityEngine;

[System.Serializable]
public struct AttackDetails
{
    public Vector2 position;

    public float damageAmount;
    public float stunDamageAmount;
    
}

[System.Serializable]
public struct WeaponAttackDetails
{
    public string attackName;

    public float movementSpeed;

    public AttackDetails attackDetails;
}
