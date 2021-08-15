using UnityEngine;

public class Enemy4 : Entity
{
    public E4_IdleState idleState { get; private set; }

    public E4_MoveState moveState { get; private set; }

    public E4_PlayerDetectedState playerDetectedState { get; private set; }
    // Need a charge?
    // public E1_ChargeState chargeState { get; private set; }

    public E4_LookForPlayerState lookForPlayerState { get; private set; }

    public E4_MeleeAttackState meleeAttackState { get; private set; }
    // need a stun?
    //public E1_StunState stunState { get; private set; }

    public E4_DeadState deadState { get; private set; }

    public E4_RangeAttackState rangeAttackState { get; private set; }

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
    private D_RangeAttack rangeAttackStateData;
    [SerializeField]
    private D_DeadState deadStateData;


    [Header("Transforms")]
    [SerializeField]
    private Transform meleeAttackPosition;

    [Header("Transforms")]
    [SerializeField]
    private Transform rangeAttackPosition;

    public override void Awake()
    {
        base.Awake();

        moveState = new E4_MoveState(this, stateMachine, "Move", moveStateData, this);
        idleState = new E4_IdleState(this, stateMachine, "Idle", idleStateData, this);
        playerDetectedState = new E4_PlayerDetectedState(this, stateMachine, "PlayerDetected", playerDetectedData, this);
       // chargeState = new E4_ChargeState(this, stateMachine, "charge", chargeStateData, this);
        lookForPlayerState = new E4_LookForPlayerState(this, stateMachine, "LookForPlayer", lookForPlayerStateData, this);
        meleeAttackState = new E4_MeleeAttackState(this, stateMachine, "MeleeAttack", meleeAttackPosition, meleeAttackStateData, this);
        rangeAttackState = new E4_RangeAttackState(this, stateMachine, "RangeAttack", rangeAttackPosition, rangeAttackStateData, this);
       // stunState = new E4_StunState(this, stateMachine, "stun", stunStateData, this);
        deadState = new E4_DeadState(this, stateMachine, "Dead", deadStateData, this);


        
    }

    private void Start()
    {
        stateMachine.Initialize(idleState);
    }

    public override void Damage(AttackDetails attackDetails)
    {
        base.Damage(attackDetails);

        if ((Core.Movement.FacingDirection == lastHitDirection
        || lastHitDirection == 0)
        && stateMachine.currentState != meleeAttackState)
        {
            Core.Movement.Flip();
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
