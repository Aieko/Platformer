using UnityEngine;

public class BlockState : State
{
    protected D_BlockState stateData;

    protected Transform blockPosition;
    

    protected bool performCloseRangeAction;
    protected bool isPlayerInMinAgroRange;
    protected bool isGrounded;
    protected bool isBlockTimeOver;
    protected bool isDetectedLedge;
    protected bool isDetectedWall;

    


    public BlockState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_BlockState stateData, Transform blockPosition) : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
        this.blockPosition = blockPosition;
    }

    public override void DoChecks()
    {
        base.DoChecks();

        performCloseRangeAction = entity.CheckPlayerInCloseRangeAction();
        isDetectedLedge = core.CollisionSenses.TouchingLedgeVertical;
        isDetectedWall = core.CollisionSenses.TouchingWallFront;
        isPlayerInMinAgroRange = entity.CheckPlayerInMinAgroRange();
        isGrounded = core.CollisionSenses.CheckIfGrounded();

    }

    public override void Enter()
    {
        base.Enter();

        isBlockTimeOver = false;

        if (stateData.shouldStay)
        {
           core.Movement.SetVelocityX(0.0f);
        }
        else
        {
           core.Movement.SetVelocityX(stateData.speedWhileBlock * core.Movement.FacingDirection);
        }

    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (Time.time >= startTime + stateData.blockTime)
        {
            isBlockTimeOver = true;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public virtual void Blocked()
    {

    }
}
