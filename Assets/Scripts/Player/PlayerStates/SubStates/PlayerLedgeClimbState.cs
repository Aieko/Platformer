﻿using UnityEngine;

public class PlayerLedgeClimbState : PlayerState
{
    private Vector2 detectedPos;
    private Vector2 cornerPos;
    private Vector2 startPos;
    private Vector2 stopPos;
    private Vector2 workspace;

    private bool isHanging;
    private bool isClimbing;
    private bool jumpInput;
    private bool isTouchingCeiling;

    private int xInput;
    private int yInput;

    public PlayerLedgeClimbState(Player player, PlayerStateMachine playerStateMachine, PlayerData playerData, string animBoolName) : base(player, playerStateMachine, playerData, animBoolName)
    {

    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        player.Anim.SetBool("ClimbLedge", false);
    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();

        if (!core.CollisionSenses.InPlatform)
        {
            isHanging = true;
        }
        
    }

    public override void Enter()
    {
        base.Enter();

        core.Movement.SetVelocityZero();
        player.transform.position = detectedPos;
        cornerPos = DetermineCornerPosition();

        startPos.Set(cornerPos.x - (core.Movement.FacingDirection * playerData.startOffset.x), cornerPos.y - playerData.startOffset.y);

        stopPos.Set(cornerPos.x + (core.Movement.FacingDirection * playerData.stopOffset.x), cornerPos.y + playerData.stopOffset.y);

        player.transform.position = startPos;
    }

    public override void Exit()
    {
        base.Exit();

        isHanging = false;

        if (!isClimbing) return;

        player.transform.position = stopPos;

        isClimbing = false;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(isAnimationFinished)
        {
            if(isTouchingCeiling)
            {
                stateMachine.ChangeState(player.CrouchIdleState);
            }
            else
            {
                stateMachine.ChangeState(player.IdleState);
            }
            
        }
        else
        {
            xInput = player.InputHandler.NormInputX;
            yInput = player.InputHandler.NormInputY;
            jumpInput = player.InputHandler.JumpInput;

            core.Movement.SetVelocityZero();
            player.transform.position = startPos;
            //check if platform moving on player and changes his state
            if(core.CollisionSenses.InPlatform)
            {
                if(core.CollisionSenses.CheckForCeiling())
                {
                    core.Movement.SetVelocityY(-10f);
                }
                stateMachine.ChangeState(player.InAirState);
            }

            if (xInput == core.Movement.FacingDirection && isHanging && !isClimbing)
            {
                CheckForSpace();
                isClimbing = true;

                player.Anim.SetBool("ClimbLedge", true);
            }
            else if (yInput == -1 && isHanging && !isClimbing)
            {
                stateMachine.ChangeState(player.InAirState);
            }
            else switch (jumpInput)
            {
                case true when !isClimbing && yInput == 1:
                    stateMachine.ChangeState(player.JumpState);
                    break;

                case true when !isClimbing && xInput != core.Movement.FacingDirection && xInput != 0:
                    player.WallJumpState.DetermineWallJumpDirection(true);
                    stateMachine.ChangeState(player.WallJumpState);
                    break;
            }
           
        } 
    }

    public void SetDetectedPosition(Vector2 pos) => detectedPos = pos;

    private void CheckForSpace()
    {
        isTouchingCeiling = Physics2D.Raycast(cornerPos + (Vector2.up * 0.015f) + (Vector2.right * core.Movement.FacingDirection * 0.015f), Vector2.up, playerData.standColliderHeight,core.CollisionSenses.WhatIsGround);
        player.Anim.SetBool("IsTouchingCeiling", isTouchingCeiling);
    }

    public Vector2 DetermineCornerPosition()
    {
        RaycastHit2D xHit = Physics2D.Raycast(core.CollisionSenses.WallCheck.position, Vector2.right * core.Movement.FacingDirection, core.CollisionSenses.WallCheckDistance, core.CollisionSenses.WhatIsGround);

        float xDist = xHit.distance;

        workspace.Set((xDist + 0.015f) * core.Movement.FacingDirection, 0f);

        RaycastHit2D yHit = Physics2D.Raycast(core.CollisionSenses.LedgeCheckHorizontal.position + (Vector3)(workspace), Vector2.down, core.CollisionSenses.LedgeCheckHorizontal.position.y - core.CollisionSenses.WallCheck.position.y + 0.015f,core.CollisionSenses.WhatIsGround);

        float yDist = yHit.distance;

        workspace.Set(core.CollisionSenses.WallCheck.position.x + (xDist * core.Movement.FacingDirection), core.CollisionSenses.LedgeCheckHorizontal.position.y - yDist);

        return workspace;
    }
}
