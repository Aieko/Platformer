using UnityEngine;

public class PlayerJumpOffState : PlayerAbilityState
{
    
    private bool isInPlatform;
    private bool isStandOnPlatform;


    public PlayerJumpOffState(Player player, PlayerStateMachine playerStateMachine, PlayerData playerData, string animBoolName) : base(player, playerStateMachine, playerData, animBoolName)
    {

    }

    private void JumpOff()
    {
         Physics2D.IgnoreLayerCollision(core.CollisionSenses.whatIsPlayer, 13, true);   
    }

    public override void DoChecks()
    {
        base.DoChecks();

        isInPlatform = core.CollisionSenses.InPlatform;
        isStandOnPlatform = core.CollisionSenses.StandOnPlatform;
    }

    public override void Enter()
    {
        base.Enter();

        player.InAirState.SetIsJumping();
        player.InputHandler.UseJumpInput();
        
        player.JumpState.DecreaseAmountOfJumpLeft();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(isStandOnPlatform)
        {
            JumpOff();
        }

        if (!(Time.time >= startTime + 0.2f) || isInPlatform) return;
        Physics2D.IgnoreLayerCollision(core.CollisionSenses.whatIsPlayer, 13, false);
        isAbilityDone = true;

    }

}
