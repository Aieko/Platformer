using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : Entity
{
    public E2_IdleState idleState { get; private set; }

    public E2_MoveState moveState { get; private set; }

    public E2_PlayerDetectedState playerDetectedState { get; private set; }

    public E2_MeleeAttackState meleeAttackState { get; private set; }

    public E2_LookForPlayerState lookForPlayerState { get; private set; }

    public E2_StunState stunState { get; private set; }

    public E2_DeadState deadState { get; private set; }

    public E2_DodgeState dodgeState { get; private set; }

    public E2_RangeAttackState rangeAttackState { get; private set; }


    [SerializeField]
    private D_IdleState idleStateData;
    [SerializeField]
    private D_MoveState moveStateData;
    [SerializeField]
    private D_PlayerDetectedState playerDetectedStateData;
    [SerializeField]
    private D_MeleeAttack meleeAttackStateData;
    [SerializeField]
    private D_LookForPlayerState lookForPlayerStateData;
    [SerializeField]
    private D_StunState stunStateData;
    [SerializeField]
    private D_DeadState deadStateData;
    [SerializeField]
    private D_RangeAttack rangeAttackData;


    public D_DodgeState dodgeStateData;

    [Header("Transforms")]
    [SerializeField]
    private Transform meleeAttackPosition;
    [SerializeField]
    private Transform rangeAttackPosition;


    public override void Start()
    {
        base.Start();

        moveState = new E2_MoveState(this, stateMachine, "Move", moveStateData, this);
        idleState = new E2_IdleState(this, stateMachine, "Idle", idleStateData, this);
        playerDetectedState = new E2_PlayerDetectedState(this, stateMachine, "PlayerDetected", playerDetectedStateData, this);
        meleeAttackState = new E2_MeleeAttackState(this, stateMachine, "MeleeAttack", meleeAttackPosition, meleeAttackStateData, this);
        lookForPlayerState = new E2_LookForPlayerState(this, stateMachine, "LookForPlayer", lookForPlayerStateData, this);
        stunState = new E2_StunState(this, stateMachine, "Stun", stunStateData, this);
        deadState = new E2_DeadState(this, stateMachine, "Dead", deadStateData, this);
        dodgeState = new E2_DodgeState(this, stateMachine, "Dodge", dodgeStateData, this);
        rangeAttackState = new E2_RangeAttackState(this, stateMachine, "RangeAttack", rangeAttackPosition, rangeAttackData, this);

        stateMachine.Initialize(moveState);
    }

    public override void Damage(AttackDetails attackDetails)
    {
        base.Damage(attackDetails);

        if ((facingDirection == lastDamageDirection
          || lastDamageDirection == 0)
          && !isStunned
          && stateMachine.currentState != meleeAttackState)
        {
            Flip();
        }

        if (isDead)
        {
            stateMachine.ChangeState(deadState);
        }


        if (isStunned && stateMachine.currentState != stunState)
        {
            stateMachine.ChangeState(stunState);
        }

        else if(CheckPlayerInMinAgroRange())
        {
            stateMachine.ChangeState(rangeAttackState);
        }


    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawWireSphere(meleeAttackPosition.position, meleeAttackStateData.attackRadius);
    }
}
