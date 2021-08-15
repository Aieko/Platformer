﻿public class PlayerCrouchMoveState : PlayerGroundState
{
    public PlayerCrouchMoveState(Player player, PlayerStateMachine playerStateMachine, PlayerData playerData, string animBoolName) : base(player, playerStateMachine, playerData, animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();

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

        if(!isExitingState)
        {
            core.Movement.SetVelocityX(playerData.crouchMovementVelocity * core.Movement.FacingDirection);
            core.Movement.CheckIfShouldFlip(xInput);
            if(xInput == 0)
            {
                stateMachine.ChangeState(player.CrouchIdleState);
            }
            else if(yInput != -1 && !isTouchingCeiling)
            {
                stateMachine.ChangeState(player.MoveState);
            }
        }
    }
}
