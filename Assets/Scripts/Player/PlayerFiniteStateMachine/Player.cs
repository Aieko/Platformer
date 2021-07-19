using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region Components
    public Core Core { get; private set; }

    public Animator Anim { get; private set; }
    public PlayerInputHandler InputHandler { get; private set; }
    public Rigidbody2D RB { get; private set; }
    public BoxCollider2D PlayerCollider { get; private set; }
    public Transform DashDirectionIndicator { get; private set; }

    public PlayerInventory Inventory { get; private set; }

    #endregion

    #region State veriables
    public PlayerStateMachine StateMachine { get; private set; }

    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerInAirState InAirState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerLandState LandedState { get; private set; }
    public PlayerWallSlideState WallSlideState { get; private set; }
    public PlayerWallGrabState WallGrabState { get; private set; }
    public PlayerWallClimbState WallClimbState { get; private set; }
    public PlayerWallJumpState WallJumpState { get; private set; }
    public PlayerLedgeClimbState LedgeClimbState { get; private set; }
    public PlayerDashState DashState { get; private set; }
    public PlayerCrouchIdleState CrouchIdleState { get; private set; }
    public PlayerCrouchMoveState CrouchMoveState { get; private set; }
    public PlayerJumpOffState JumpOffState { get; private set; }
    
    public PlayerAttackState PrimaryAttackState { get; private set; }
    public PlayerAttackState SecondaryAttackState { get; private set; }

    [SerializeField, Header("Data")]
    private PlayerData playerData;
    
    #endregion

    #region Public Veriables

    

    
    #endregion

    

    #region Private Verialbes
    private Vector2 workspace;
    
    #endregion

    #region Unity Callback Functions
    private void Awake()
    {
        Core = GetComponentInChildren<Core>();

        StateMachine = new PlayerStateMachine();

        IdleState = new PlayerIdleState(this, StateMachine, playerData, "Idle");
        MoveState = new PlayerMoveState(this, StateMachine, playerData, "Move");
        JumpState = new PlayerJumpState(this, StateMachine, playerData, "InAir");
        InAirState = new PlayerInAirState(this, StateMachine, playerData, "InAir");
        LandedState = new PlayerLandState(this, StateMachine, playerData, "Landed");
        WallSlideState = new PlayerWallSlideState(this, StateMachine, playerData, "WallSlide");
        WallGrabState = new PlayerWallGrabState(this, StateMachine, playerData, "WallGrab");
        WallClimbState = new PlayerWallClimbState(this, StateMachine, playerData, "WallClimb");
        WallJumpState = new PlayerWallJumpState(this, StateMachine, playerData, "InAir");
        LedgeClimbState = new PlayerLedgeClimbState(this, StateMachine, playerData, "LedgeClimbState");
        DashState = new PlayerDashState(this, StateMachine, playerData, "InAir");
        CrouchIdleState = new PlayerCrouchIdleState(this, StateMachine, playerData, "CrouchIdle");
        CrouchMoveState = new PlayerCrouchMoveState(this, StateMachine, playerData, "CrouchMove");
        JumpOffState = new PlayerJumpOffState(this, StateMachine, playerData, "InAir");
        PrimaryAttackState = new PlayerAttackState(this, StateMachine, playerData, "Attack");
        SecondaryAttackState = new PlayerAttackState(this, StateMachine, playerData, "Attack");
    }

    private void Start()
    {
        //COMPONENTS
        Anim = GetComponent<Animator>();
        InputHandler = GetComponent<PlayerInputHandler>();
        RB = GetComponent<Rigidbody2D>();
        PlayerCollider = GetComponent<BoxCollider2D>();
        DashDirectionIndicator = transform.Find("DashDirectionIndicator");

        //INVENTORY
        Inventory = GetComponent<PlayerInventory>();

        PrimaryAttackState.SetWeapon(Inventory.weapons[(int)CombatInputs.primary]);
        SecondaryAttackState.SetWeapon(Inventory.weapons[(int)CombatInputs.secondary]);
        
        StateMachine.Initialize(IdleState); 
    }

    private void Update()
    {
        Core.LogicUpdate();
        StateMachine.currentState.LogicUpdate();        
    }

    private void FixedUpdate()
    {
        StateMachine.currentState.PhysicsUpdate();
    }

    #endregion
    

    #region Other Functions

    public void SetColliderHeight(float height)
    {
        Vector2 center = PlayerCollider.offset;

        workspace.Set(PlayerCollider.size.x, height);

        center.y += (height - PlayerCollider.size.y) / 2;

        PlayerCollider.size = workspace;

        PlayerCollider.offset = center;
    }

    private void AnimationTrigger() => StateMachine.currentState.AnimationTrigger();

    private void AnimationFinishTrigger() => StateMachine.currentState.AnimationFinishTrigger();

    private void InvalidOperationException(string ex)
    {
        Debug.LogError(ex);
    }
    #endregion

}
