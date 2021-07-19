using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundState : PlayerState
{
    protected int xInput;
    protected int yInput;

    protected bool isTouchingCeiling;
    private bool jumpInput;
    private bool IsGrounded;
    private bool isTouchingWall;
    private bool isTouchingLedge;
    private bool isStandOnPlatform;
    private bool grabInput;
    private bool dashInput;


    public PlayerGroundState(Player player, PlayerStateMachine playerStateMachine, PlayerData playerData, string animBoolName) : base(player, playerStateMachine, playerData, animBoolName)
    {

    }

    public override void DoChecks()
    {
        base.DoChecks();

        IsGrounded = core.CollisionSenses.CheckIfGrounded();
        isTouchingCeiling = core.CollisionSenses.CheckForCeiling();
        isTouchingWall = core.CollisionSenses.TouchingWallFront;
        isTouchingLedge = core.CollisionSenses.TouchingLedgeHorizontal;
        isStandOnPlatform = core.CollisionSenses.StandOnPlatform;
    }

    public override void Enter()
    {
        base.Enter();

        player.JumpState.ResetAmountOfJumps();
        player.DashState.ResetCanDash();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        xInput = player.InputHandler.NormInputX;
        yInput = player.InputHandler.NormInputY;
        jumpInput = player.InputHandler.JumpInput;
        grabInput = player.InputHandler.GrabInput;
        dashInput = player.InputHandler.DashInput;


        if(player.InputHandler.AttackInputs[(int)CombatInputs.primary] && !isTouchingCeiling)
        {
            stateMachine.ChangeState(player.PrimaryAttackState);
        }
        else if (player.InputHandler.AttackInputs[(int)CombatInputs.secondary] && !isTouchingCeiling)
        {
            stateMachine.ChangeState(player.SecondaryAttackState);
        }
        else if (jumpInput && player.InputHandler.NormInputY == -1 && isStandOnPlatform)
        {
            stateMachine.ChangeState(player.JumpOffState);
        }
        else if (jumpInput && player.JumpState.CanJump() && !isTouchingCeiling)
        { 
            stateMachine.ChangeState(player.JumpState);
        }
        else if(!IsGrounded)
        {
            player.InAirState.StartCoyoteTime();
            stateMachine.ChangeState(player.InAirState);
        }
        else if(isTouchingWall && grabInput && isTouchingLedge && !isTouchingCeiling)
        {
            stateMachine.ChangeState(player.WallGrabState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
