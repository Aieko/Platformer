using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region Components
    public Animator Anim { get; private set; }
    public PlayerInputHandler InputHandler { get; private set; }
    public Rigidbody2D RB { get; private set; }
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

    [SerializeField,Header("Data")]
    private PlayerData playerData;
    #endregion

    #region Public Veriables

    public int FacingDirection { get; private set; }
    public Vector2 CurrentVelocity { get; private set; }
    #endregion

    #region Check Transfroms
    [Header("Check Tranforms")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform wallCheck;
    #endregion

    #region Private Verialbes
    private Vector2 workspace;
    #endregion

    #region Unity Callback Functions
    private void Awake()
    {
        StateMachine = new PlayerStateMachine();
        InputHandler = GetComponent<PlayerInputHandler>();
        RB = GetComponent<Rigidbody2D>();


        IdleState = new PlayerIdleState(this, StateMachine, playerData, "Idle");
        MoveState = new PlayerMoveState(this, StateMachine, playerData, "Move");
        JumpState = new PlayerJumpState(this, StateMachine, playerData, "InAir");
        InAirState = new PlayerInAirState(this, StateMachine, playerData, "InAir");
        LandedState = new PlayerLandState(this, StateMachine, playerData, "Landed");
        WallSlideState = new PlayerWallSlideState(this, StateMachine, playerData, "WallSlide");
        WallGrabState = new PlayerWallGrabState(this, StateMachine, playerData, "WallGrab");
        WallClimbState = new PlayerWallClimbState(this, StateMachine, playerData, "WallClimb");

    }

    private void Start()
    {
        Anim = GetComponent<Animator>();
        FacingDirection = 1;
        StateMachine.Initialize(IdleState);
    }

    private void Update()
    {
        CurrentVelocity = RB.velocity;
        StateMachine.currentState.LogicUpdate();
    }

    private void FixedUpdate()
    {
        StateMachine.currentState.PhysicsUpdate();
    }
    #endregion

    #region Set Functions
    public void SetVelocityX(float velocity)
    {
        workspace.Set(velocity, CurrentVelocity.y);
        RB.velocity = workspace;
        CurrentVelocity = workspace;
    }

    public void SetVelocityY(float velocity)
    {
        workspace.Set(CurrentVelocity.x, velocity);
        RB.velocity = workspace;
        CurrentVelocity = workspace;
    }
    #endregion

    #region Check Functions

    public void CheckIfShouldFlip(int xInput)
    {
        if(xInput != 0 && xInput != FacingDirection)
        {
            Flip();
        }
    }

    public bool CheckIfGrounded() => Physics2D.OverlapCircle(groundCheck.position, playerData.GroundCheckRadius, playerData.whatIsGround);

    public bool CheckIfTouchingWall() => Physics2D.Raycast(wallCheck.position, Vector2.right * FacingDirection, playerData.wallCheckDistance, playerData.whatIsGround);

    #endregion

    #region Other Functions
    private void Flip()
    {
        FacingDirection *= -1;
        transform.Rotate(0.0f, 180.0f, 0.0f);
    }

    private void AnimationTrigger() => StateMachine.currentState.AnimationTrigger();

    private void AnimationFinishTrigger() => StateMachine.currentState.AnimationFinishTrigger();

    #endregion

}
