using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy3 : Entity

{
    public E3_IdleState idleState { get; private set; }

    public E3_MoveState moveState { get; private set; }

    public E3_PlayerDetectedState playerDetectedState { get; private set; }

    public E3_LookForPlayerState lookForPlayerState { get; private set; }

    public E3_MeleeAttackState meleeAttackState { get; private set; }

    public E3_MeleeAttack2State meleeAttack2State { get; private set; }

    public E3_DeadState deadState { get; private set; }

    public E3_ChargeState chargeState { get; private set; }

    public E3_BlockState blockState { get; private set; }

    

    [Header("State's Data")]
    [SerializeField]
    private D_IdleState idleStateData;
    [SerializeField]
    private D_MoveState moveStateData;
    [SerializeField]
    private D_PlayerDetectedState playerDetectedData;
    [SerializeField]
    private D_LookForPlayerState lookForPlayerStateData;
    [SerializeField]
    private D_MeleeAttack meleeAttackStateData;
    [SerializeField]
    private D_MeleeAttack meleeAttack2StateData;
    [SerializeField]
    private D_DeadState deadStateData;
    [SerializeField]
    private D_ChargeState chargeStateData;
    [SerializeField]
    private D_BlockState blockStateData;

    [Header("Transforms")]
    [SerializeField]
    private Transform meleeAttackPosition;
    [SerializeField]
    private Transform blockPosition;

    [HideInInspector]
    public bool needToCounterAttack;

    private int hitDirection;

    private int provokeToBlock = 0;

    public override void Start()
    {
        base.Start();

        moveState = new E3_MoveState(this, stateMachine, "Move", moveStateData, this);
        idleState = new E3_IdleState(this, stateMachine, "Idle", idleStateData, this);
        playerDetectedState = new E3_PlayerDetectedState(this, stateMachine, "PlayerDetected", playerDetectedData, this);
        lookForPlayerState = new E3_LookForPlayerState(this, stateMachine, "LookForPlayer", lookForPlayerStateData, this);
        meleeAttackState = new E3_MeleeAttackState(this, stateMachine, "MeleeAttack", meleeAttackPosition, meleeAttackStateData, this);
        meleeAttack2State = new E3_MeleeAttack2State(this, stateMachine, "MeleeAttack2", meleeAttackPosition, meleeAttack2StateData, this);
        deadState = new E3_DeadState(this, stateMachine, "Dead", deadStateData, this);
        chargeState = new E3_ChargeState(this, stateMachine, "Charge", chargeStateData, this);
        blockState = new E3_BlockState(this, stateMachine, "Block", blockStateData, blockPosition, this);

        stateMachine.Initialize(idleState);
    }

    public override void Damage(AttackDetails attackDetails)
    {
        if (attackDetails.position.x > aliveGO.transform.position.x)
        {
            hitDirection = -1;
        }
        else
        {
            hitDirection = 1;
        }

        if ((facingDirection == hitDirection)
           && stateMachine.currentState != meleeAttackState
           && stateMachine.currentState != blockState)
        {
            Flip();
        }

        if (stateMachine.currentState == blockState
            && facingDirection != hitDirection)
        {
            blockState.Blocked();
        }   
        else if (stateMachine.currentState != blockState && facingDirection != hitDirection)
        {
                base.Damage(attackDetails);

            if(provokeToBlock == 1)
            {
                stateMachine.ChangeState(blockState);

                provokeToBlock = 0;

                return;
            }
                provokeToBlock++;
                needToCounterAttack = true; 
        }
        else if (facingDirection == hitDirection)
        {
            base.Damage(attackDetails);

            Flip();
        }

        if (isDead)
        {
            stateMachine.ChangeState(deadState);
        }


    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawWireSphere(meleeAttackPosition.position, meleeAttackStateData.attackRadius);
    }
}
