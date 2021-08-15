using UnityEngine;

[CreateAssetMenu(fileName = "NewDeadStateData", menuName = "Data/State Data/Dead State")]
public class D_DeadState : ScriptableObject
{
    public GameObject deathChunkParticles;

    public GameObject deathBloodParticles;
}
