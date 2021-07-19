using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionSenses : CoreComponent
{
    public LayerMask WhatIsGround { get => whatIsGround; set => whatIsGround = value; }

    public Transform GroundCheck
    {
        get
        {
            if(groundCheck)
                return groundCheck;

            Debug.LogError("No Ground Check on" + core.transform.parent.name);
            return null;
        }

            set => groundCheck = value;

    }

    public Transform WallCheck
    {
        get
        {
            if (wallCheck)
                return wallCheck;

            Debug.LogError("No Wall Check on" + core.transform.parent.name);
       
            return null;
        }

        set => wallCheck = value;
    }

    public Transform LedgeCheckHorizontal
    {
        get
        {
            if (ledgeCheckHorizontal)
                return ledgeCheckHorizontal;

            Debug.LogError("No Ledge Check Horizontal on" + core.transform.parent.name);
            return null;
        }

        set => ledgeCheckHorizontal = value;
    }

    public Transform CeilingCheck {
        get
        {
            if (ceilingCheck)
                return ceilingCheck;

            Debug.LogError("No Ceiling Check on" + core.transform.parent.name);
            return null;
        }

        set => ceilingCheck = value;
    }

    public Transform LedgeCheckVertical {
        get
        {
            if (ledgeCheckVertical)
                return ledgeCheckVertical;

            Debug.LogError("No Ledge Check Vertical on" + core.transform.parent.name);
            return null;
        }

        set => ledgeCheckVertical = value;
    }

    public float GroundCheckRadius { get => groundCheckRadius; set => groundCheckRadius = value; }
    public float WallCheckDistance { get => wallCheckDistance; set => wallCheckDistance = value; }

    public Vector2 CollisionCheckBoxSize { get => collisionCheckBoxSize; set => collisionCheckBoxSize = value; }


    #region Check Transfroms
    [Header("Check Tranforms")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private Transform ledgeCheckHorizontal;
    [SerializeField] private Transform ledgeCheckVertical;
    [SerializeField] private Transform ceilingCheck;
    #endregion

    
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private LayerMask whatIsPlatform;
   
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
    }

    #region Check Functions

    public bool CheckIfGrounded()
    {
        if (Physics2D.OverlapCircle(GroundCheck.position, groundCheckRadius, whatIsGround)
         || Physics2D.OverlapCircle(GroundCheck.position, groundCheckRadius, whatIsPlatform))
        {
            return true;
        }
        else
            return false;
    }

    public bool StandOnPlatform { get => Physics2D.OverlapCircle(GroundCheck.position, groundCheckRadius, whatIsPlatform); }

    public bool TouchingWallFront { get => Physics2D.Raycast(WallCheck.position, Vector2.right * core.Movement.FacingDirection, wallCheckDistance, whatIsGround); }

    public bool TouchingWallBack { get => Physics2D.Raycast(WallCheck.position, Vector2.right * -core.Movement.FacingDirection, wallCheckDistance, whatIsGround); }

    public bool TouchingLedgeHorizontal { get => Physics2D.Raycast(LedgeCheckHorizontal.position, Vector2.right * core.Movement.FacingDirection, wallCheckDistance, whatIsGround); }

    public bool TouchingLedgeVertical { get => Physics2D.Raycast(LedgeCheckVertical.position, Vector2.down, wallCheckDistance, whatIsGround); }

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

    public bool InPlatfrom { get => Physics2D.OverlapBox(WallCheck.position, collisionCheckBoxSize, 0, whatIsPlatform); }

  

    public void CheckToIgnorePlatformCollision()
    {
        if (core.Movement.CurrentVelocity.y > 0)
        {
            Collider2D collider = Physics2D.OverlapCircle(LedgeCheckHorizontal.position, groundCheckRadius, playerLayerToIgnore);

            if (collider)
            {
                if (collider.CompareTag("JumpOffPlatform"))
                {
                    Physics2D.IgnoreLayerCollision(whatIsPlayer, 13, true);
                }

            }
        }
        else if (core.Movement.CurrentVelocity.y <= 0 && !InPlatfrom)
        {
            Physics2D.IgnoreLayerCollision(whatIsPlayer, 13, false);
        }
    }

    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(WallCheck.position, collisionCheckBoxSize);
        
      //Gizmos.DrawLine(LedgeCheckHorizontal.position, new Vector3(LedgeCheckHorizontal.position.x, LedgeCheckHorizontal.position.y + 0.8f));
    }
}
