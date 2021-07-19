using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrouchIdleState : PlayerGroundState
{
    public PlayerCrouchIdleState(Player player, PlayerStateMachine playerStateMachine, PlayerData playerData, string animBoolName) : base(player, playerStateMachine, playerData, animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();

        core.Movement.SetVelocityZero();
        player.SetColliderHeight(playerData.crouchColliderHeight);
       
    }

    public override void Exit()
    {
        base.Exit();
        player.SetColliderHeight(playerData.standColliderHeight);
        
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!isExitingState)
        {
            if (xInput != 0)
            {
                stateMachine.ChangeState(player.CrouchMoveState);
            }
            else if (yInput != -1 && !isTouchingCeiling)
            {
                stateMachine.ChangeState(player.IdleState);
            }
    }
}
}
