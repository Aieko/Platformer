using UnityEngine;

[CreateAssetMenu(fileName = "NewStunStateData", menuName = "Data/State Data/Stun State")]
public class D_StunState : ScriptableObject
{
    public float stunTime = 3f;

    public float stunKnockbackTime = 0.2f;

    public Vector2 stunKnockbackAngle;

    public float stunKnockbackSpeed = 20f;
}
