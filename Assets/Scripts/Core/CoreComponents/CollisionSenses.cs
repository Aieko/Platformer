using UnityEngine;

public class CollisionSenses : CoreComponent
{
    public LayerMask WhatIsGround { get => whatIsGround; set => whatIsGround = value; }

    public Transform GroundCheck
    {
        get => GenericNotImplementedError<Transform>.TryGet(groundCheck, transform.parent.name);

        set => groundCheck = value;

    }

    public Transform WallCheck
    {
        get => GenericNotImplementedError<Transform>.TryGet(wallCheck, transform.parent.name);

        set => wallCheck = value;
    }

    public Transform LedgeCheckHorizontal
    {
        get => GenericNotImplementedError<Transform>.TryGet(ledgeCheckHorizontal, transform.parent.name);

        set => ledgeCheckHorizontal = value;
    }

    public Transform CeilingCheck
    {
        get => GenericNotImplementedError<Transform>.TryGet(ceilingCheck, transform.parent.name);

        set => ceilingCheck = value;
    }

    public Transform LedgeCheckVertical
    {
        get => GenericNotImplementedError<Transform>.TryGet(ledgeCheckVertical, transform.parent.name);

        set => ledgeCheckVertical = value;
    }

    public float GroundCheckRadius { get => groundCheckRadius; set => groundCheckRadius = value; }

    public float WallCheckDistance { get => wallCheckDistance; set => wallCheckDistance = value; }

    public Vector2 CollisionCheckBoxSize { get => collisionCheckBoxSize; set => collisionCheckBoxSize = value; }

    private GameObject secretWorkspace;
    private float ledgeCheckDistance;

    #region Check Transfroms
    [Header("Check Transforms")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private Transform ledgeCheckHorizontal;
    [SerializeField] private Transform ledgeCheckVertical;
    [SerializeField] private Transform ceilingCheck;
    #endregion

    
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private LayerMask whatIsPlatform;
    [SerializeField] private LayerMask whatIsSecret;
   
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private float groundCheckRadius;

    [SerializeField] private Vector2 collisionCheckBoxSize;

    public int playerLayerToIgnore { get; private set; }
    public LayerMask whatIsPlayer { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        whatIsPlayer = LayerMask.NameToLayer("Player");
        playerLayerToIgnore = 1 << LayerMask.NameToLayer("Player");
        playerLayerToIgnore = ~playerLayerToIgnore;
        ledgeCheckDistance = wallCheckDistance;
    }

    #region Check Functions

    public bool CheckIfGrounded() { return StandOnGround || StandOnPlatform; }

    public bool StandOnPlatform => Physics2D.OverlapCircle(GroundCheck.position, groundCheckRadius, whatIsPlatform);

    public bool StandOnGround => Physics2D.OverlapCircle(GroundCheck.position, groundCheckRadius, whatIsGround);

    public bool TouchingWallFront => Physics2D.Raycast(WallCheck.position, Vector2.right * core.Movement.FacingDirection, wallCheckDistance, whatIsGround);

    public bool TouchingWallBack => Physics2D.Raycast(WallCheck.position, Vector2.right * -core.Movement.FacingDirection, wallCheckDistance, whatIsGround);

    public bool TouchingLedgeHorizontal => Physics2D.Raycast(LedgeCheckHorizontal.position, Vector2.right * core.Movement.FacingDirection, ledgeCheckDistance, whatIsGround);

    public bool TouchingLedgeVertical => Physics2D.Raycast(LedgeCheckVertical.position, Vector2.down, wallCheckDistance, whatIsGround);

    public bool CheckForCeiling()
    {
        if (Physics2D.OverlapCircle(CeilingCheck.position, groundCheckRadius, whatIsGround)
         || Physics2D.OverlapCircle(CeilingCheck.position, groundCheckRadius, whatIsPlatform))
        {
            return true;
        }
        else
            return false;

    }

    public bool InPlatform => Physics2D.OverlapBox(WallCheck.position, collisionCheckBoxSize, 0, whatIsPlatform);

    public void CheckToIgnorePlatformCollision()
    {
        if (core.Movement.CurrentVelocity.y > 0)
        {
            var collider = Physics2D.OverlapCircle(LedgeCheckHorizontal.position, groundCheckRadius, playerLayerToIgnore);

            if (collider)
            {
                if (collider.CompareTag("JumpOffPlatform"))
                {
                    Physics2D.IgnoreLayerCollision(whatIsPlayer, 13, true);
                }

            }
        }
        else if (core.Movement.CurrentVelocity.y <= 0 && !InPlatform)
        {
            Physics2D.IgnoreLayerCollision(whatIsPlayer, 13, false);
        }
    }

    #endregion

    #region Other Methods

    public void SetLedgeCheck(bool value)
    {
       wallCheckDistance = value ? wallCheckDistance : 0f;
    }

    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(WallCheck.position, collisionCheckBoxSize);
        
      //Gizmos.DrawLine(LedgeCheckHorizontal.position, new Vector3(LedgeCheckHorizontal.position.x, LedgeCheckHorizontal.position.y + 0.8f));
    }
}
