using UnityEngine;

public class E4_RangeAttackState : RangeAttackState
{
    Enemy4 enemy;
    public E4_RangeAttackState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Transform attackPosition, D_RangeAttack stateData, Enemy4 enemy)
        : base(entity, stateMachine, animBoolName, attackPosition, stateData)
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

    public override void FinishAttack()
    {
        base.FinishAttack();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isAnimationFinished)
        {
            if (isPlayerInMinAgroRange)
            {
                stateMachine.ChangeState(enemy.playerDetectedState);
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

    public override void TriggerAttack()
    {
        base.TriggerAttack();
        
       GameObject player = GameObject.FindGameObjectWithTag("Player");

        if(player)
        {   
            float travelDistance = 0;
            float distance = Mathf.Abs((Mathf.Abs(enemy.transform.position.x) - Mathf.Abs(player.transform.position.x)));

            if (distance > 7.5f)
            {
                travelDistance = 2.2f;
            }
            else if (distance < 7.5f)
            {
                travelDistance = 1;

                if (distance < 5f)
                    travelDistance = 0;
            }
            projectileScript.FireProjectile(stateData.projectileSpeed, travelDistance, stateData.projectileDamage);
        }
       
    }
}
