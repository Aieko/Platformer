using UnityEngine;

[CreateAssetMenu(fileName = "NewDodgeStateData", menuName = "Data/State Data/Dodge State")]
public class D_DodgeState : ScriptableObject
{

    public float dodgeSpeed = 10f;
    public float dodgeTime = 0.2f;
    public float dodgeCoolDown = 2f;

    public Vector2 dodgeAngle;
    
}
