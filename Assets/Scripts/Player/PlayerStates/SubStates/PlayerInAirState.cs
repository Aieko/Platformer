using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInAirState : PlayerState
{
    #region Checks
    private bool isTouchingLedge;
    private bool isTouchingWall;
    private bool isTouchingWallBack;
    private bool isJumping;
    private bool isGrounded;
    //old, not deprecated
    private bool oldIsTouchingWall;
    private bool oldIsTouchingWallBack;
    #endregion

    #region Input
    private int xInput;
    private bool dashInput;
    private bool grabInput;
    private bool JumpInput;
    private bool jumpInputStop;
    #endregion
    
    private bool coyoteTime;
    private bool WallJumpCoyoteTime;
    private bool jumpInputCatch;
    private float startWallJumpCoyoteTime;
    

    public PlayerInAirState(Player player, PlayerStateMachine playerStateMachine, PlayerData playerData, string animBoolName) : base(player, playerStateMachine, playerData, animBoolName)
    {

    }

    public override void DoChecks()
    {
        base.DoChecks();

        oldIsTouchingWall = isTouchingWall;
        oldIsTouchingWallBack = isTouchingWallBack;

        isGrounded = core.CollisionSenses.CheckIfGrounded();
        isTouchingWall = core.CollisionSenses.TouchingWallFront;
        isTouchingWallBack = core.CollisionSenses.TouchingWallBack;
        isTouchingLedge = core.CollisionSenses.TouchingLedgeHorizontal;


        if (isTouchingWall && !isTouchingLedge)
        {
            player.LedgeClimbState.SetDetectedPosition(player.transform.position);
        }

        if (!WallJumpCoyoteTime && !isTouchingWall && !isTouchingWallBack
            && (oldIsTouchingWall || oldIsTouchingWallBack || WallJumpCoyoteTime))
        {
            StartWallJumpCoyoteTime();
        }

    }

    public override void Enter()
    {
        base.Enter();

        
    }

    public override void Exit()
    {
        base.Exit();

        oldIsTouchingWall = false;
        oldIsTouchingWallBack = false;
        isTouchingWall = false;
        isTouchingWallBack = false;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();



        CheckCoyoteTime();
        CheckWallJumpCoyoteTime();

        xInput = player.InputHandler.NormInputX;
        JumpInput = player.InputHandler.JumpInput;
        jumpInputStop = player.InputHandler.JumpInputStop;
        grabInput = player.InputHandler.GrabInput;
        dashInput = player.InputHandler.DashInput;
       

        CheckJumpMultiplier();

        //Jumping through the platforms
        //TODO need to make two different objects - "dynamic platforms" and "static platforms" 
        //so player could climb through the static and be stunned by dynamic if they start to collide with his head

        core.CollisionSenses.CheckToIgnorePlatformCollision();

        if (player.InputHandler.AttackInputs[(int)CombatInputs.primary])
        {
            stateMachine.ChangeState(player.PrimaryAttackState);
        }
        else if (player.InputHandler.AttackInputs[(int)CombatInputs.secondary])
        {
            stateMachine.ChangeState(player.SecondaryAttackState);
        }
        else if (isGrounded && core.Movement.CurrentVelocity.y < 0.01f && !core.CollisionSenses.InPlatfrom)
        {
            stateMachine.ChangeState(player.LandedState);
        }
        else if(isTouchingWall && !isTouchingLedge && !isGrounded)
        {
            stateMachine.ChangeState(player.LedgeClimbState);
        }
        else if((isTouchingWall || isTouchingWallBack) && JumpInput)
        {
            StopWallJumpCoyoteTime();
            isTouchingWall = core.CollisionSenses.TouchingWallFront;
            player.WallJumpState.DetermineWallJumpDirection(isTouchingWall);
            stateMachine.ChangeState(player.WallJumpState);
        }
        else if (JumpInput && player.JumpState.CanJump())
        {
            stateMachine.ChangeState(player.JumpState); 
        }
        else if(isTouchingWall && grabInput && isTouchingLedge)
        {
            stateMachine.ChangeState(player.WallGrabState);
        }
        else if(isTouchingWall && xInput == core.Movement.FacingDirection && core.Movement.CurrentVelocity.y <= 0f)
        {
            stateMachine.ChangeState(player.WallSlideState);
        }

        else if(dashInput && player.DashState.CheckIfCanDash())
        {
            stateMachine.ChangeState(player.DashState);
        }
        else

        {
            core.Movement.CheckIfShouldFlip(xInput);
            core.Movement.SetVelocityX(playerData.movementVelocity * xInput);

            player.Anim.SetFloat("yVelocity", core.Movement.CurrentVelocity.y);
            player.Anim.SetFloat("xVelocity", Mathf.Abs(core.Movement.CurrentVelocity.x));
        }

    }

    private void CheckJumpMultiplier()
    {

        if (isJumping)
        {
            if (jumpInputStop)
            {
                core.Movement.SetVelocityY(core.Movement.CurrentVelocity.y * playerData.jumpHeightMultiplier);
                isJumping = false;
            }
            else if (core.Movement.CurrentVelocity.y <= 0)
            {
                isJumping = false;
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    private void CheckCoyoteTime()
    {
        
        if(coyoteTime && Time.time > startTime + playerData.coyoteTime)
        {
            if(!isJumping)
            {
                coyoteTime = false;
                player.JumpState.DecreaseAmountOfJumpLeft();
            }
            else
            {
                coyoteTime = false;
            }
        }
        
    }

    private void CheckWallJumpCoyoteTime()
    {
        if(WallJumpCoyoteTime && Time.time > startWallJumpCoyoteTime + playerData.coyoteTime)
        {
            WallJumpCoyoteTime = true;
        }
    }

    public void StartCoyoteTime() => coyoteTime = true;

    public void StartWallJumpCoyoteTime()
    {
        WallJumpCoyoteTime = true;
        startWallJumpCoyoteTime = Time.time;
    }

    public void StopWallJumpCoyoteTime() => WallJumpCoyoteTime = false;

    public void SetIsJumping() => isJumping = true;

}
