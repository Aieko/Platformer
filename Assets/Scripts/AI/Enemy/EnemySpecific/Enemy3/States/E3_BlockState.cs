using UnityEngine;

public class E3_BlockState : BlockState
{
    Enemy3 enemy;

    public E3_BlockState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_BlockState stateData, Transform blockPosition, Enemy3 enemy) : 
        base(entity, stateMachine, animBoolName, stateData, blockPosition)
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

        enemy.needToCounterAttack = false;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

             if (!isDetectedLedge || isDetectedWall)
            {
                stateMachine.ChangeState(enemy.lookForPlayerState);
            }

             else if(isBlockTimeOver)
                 {
                    if (performCloseRangeAction)
                    {
                        stateMachine.ChangeState(enemy.meleeAttackState);
                    }
                    else if (isPlayerInMinAgroRange)
                    {
                        stateMachine.ChangeState(enemy.chargeState);
                    }
                    else
                    {
                        stateMachine.ChangeState(enemy.lookForPlayerState);
                    }
                 }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void Blocked()
    {
        base.Blocked();

        enemy.anim.SetTrigger("Blocked");
        GameObject.Instantiate(stateData.hitParticle, blockPosition.position, Quaternion.Euler(0.0f, 0.0f, Random.Range(0.0f, 360.0f)));

        if(stateData.shouldConterAttack && enemy.needToCounterAttack)
        {
            stateMachine.ChangeState(enemy.meleeAttack2State);
        }

        enemy.needToCounterAttack = false;

    }
}
