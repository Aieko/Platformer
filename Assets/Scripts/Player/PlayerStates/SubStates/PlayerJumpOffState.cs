using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpOffState : PlayerAbilityState
{
    public bool jumpOffCoroutineIsRunning { get; private set; }

    private bool isInPlatfrom;
    private bool isStandOnPlatfrom;
    

    public PlayerJumpOffState(Player player, PlayerStateMachine playerStateMachine, PlayerData playerData, string animBoolName) : base(player, playerStateMachine, playerData, animBoolName)
    {

    }

    void JumpOff()
    {
         Physics2D.IgnoreLayerCollision(core.CollisionSenses.whatIsPlayer, 13, true);   
    }

    public override void DoChecks()
    {
        base.DoChecks();

        isInPlatfrom = core.CollisionSenses.InPlatfrom;
        isStandOnPlatfrom = core.CollisionSenses.StandOnPlatform;
    }

    public override void Enter()
    {
        base.Enter();

        player.InAirState.SetIsJumping();
        player.InputHandler.UseJumpInput();
        
        player.JumpState.DecreaseAmountOfJumpLeft();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(isStandOnPlatfrom)
        {
            JumpOff();
        }

        if(Time.time >= startTime + 0.2f && !isInPlatfrom)
        {
            Physics2D.IgnoreLayerCollision(core.CollisionSenses.whatIsPlayer, 13, false);
            isAbilityDone = true;
        }
       
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
