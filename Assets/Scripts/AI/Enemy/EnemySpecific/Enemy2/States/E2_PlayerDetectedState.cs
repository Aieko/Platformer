﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E2_PlayerDetectedState : PlayerDetectedState
{
    Enemy2 enemy;
    public E2_PlayerDetectedState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_PlayerDetectedState stateData, Enemy2 enemy) : base(entity, stateMachine, animBoolName, stateData)
    {
        this.enemy = enemy;
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (performCloseRangeAction)
        {
            if(Time.time >= enemy.dodgeState.startTime + enemy.dodgeStateData.dodgeCoolDown)
            {
                stateMachine.ChangeState(enemy.dodgeState);
            }
            else
            {
                stateMachine.ChangeState(enemy.meleeAttackState);
            }
           
        }
        else if(performLongRangeAction)
        {
            stateMachine.ChangeState(enemy.rangeAttackState);
        }
        else
        if(!isPlayerInMaxAgroRange)
        {
            stateMachine.ChangeState(enemy.lookForPlayerState);
        }

        
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
