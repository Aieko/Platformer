using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeAttackState : AttackState
{
    protected D_RangeAttack stateData;

    protected AttackDetails attackDetails;

    protected GameObject projectile;

    protected Projectile projectileScript;

 
    public RangeAttackState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Transform attackPosition, D_RangeAttack stateData) : 
        base(entity, stateMachine, animBoolName, attackPosition)
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

        
        attackDetails.damageAmount = stateData.projectileDamage;
        attackDetails.position = entity.aliveGO.transform.position;
        
        var angle = Mathf.Atan2(stateData.launchAngle, entity.facingDirection) * Mathf.Rad2Deg;
       attackPosition.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

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

    public override void TriggerAttack()
    {
        base.TriggerAttack();

        projectile = GameObject.Instantiate(stateData.projectile, attackPosition.position, attackPosition.rotation);

        projectileScript = projectile.GetComponent<Projectile>();

       
    }

    public override void FinishAttack()
    {
        base.FinishAttack();
    }
}
