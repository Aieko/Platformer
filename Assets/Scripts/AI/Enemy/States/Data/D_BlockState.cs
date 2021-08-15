using UnityEngine;
[CreateAssetMenu(fileName = "NewBlockStateData", menuName = "Data/State Data/Block State")]
public class D_BlockState : ScriptableObject
{

    public float blockTime = 1f;

    public GameObject hitParticle;

    public bool shouldStay = true;

    public float speedWhileBlock;

    public bool shouldConterAttack = false;

    public float counterAttackTimeAfterBlock = 0.2f;

}


