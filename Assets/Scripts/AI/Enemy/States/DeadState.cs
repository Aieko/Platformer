using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : State
{
    protected D_DeadState stateData;
    public DeadState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_DeadState stateData) : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
        
        if(stateData.deathBloodParticles && stateData.deathChunkParticles)
        {
            GameObject.Instantiate(stateData.deathBloodParticles, entity.aliveGO.transform.position, stateData.deathBloodParticles.transform.rotation);
            GameObject.Instantiate(stateData.deathChunkParticles, entity.aliveGO.transform.position, stateData.deathChunkParticles.transform.rotation);
        }
        entity.enabled = false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
