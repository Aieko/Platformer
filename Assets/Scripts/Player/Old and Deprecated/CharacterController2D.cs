using System.Collections;
using UnityEngine;

public class CharacterController2D : MonoBehaviour
{

    private float movementInputDirection;
    private float jumpTimer;
    private float turnTimer;
    private float wallJumpTimer;
    private float dashTimeLeft;
    private float lastImageXPos;
    private float lastDash = -100f;
    private float knockbackStartTime;

    private int amountOfJumpsLeft;
    private int facingDirection = 1;
    private int lastWallJumpDirection;

    private bool isFacingRight = true;
    private bool isWalking;
    private bool knockback;
    private bool isTouchingWall;
    private bool isWallSliding;
    private bool canNormalJump;
    private bool canWallJump;
    private bool isAttemptingToJump;
    private bool checkJumpMultiplier;
    private bool canMove;
    private bool canFlip;
    private bool hasWallJumped;
    private bool isTouchingLedge;
    private bool canClimbLedge = false;
    private bool canClimb = true;
    private bool ledgeDetected;
    private bool canDash = true;
    private bool jumpOffCoroutineIsRunning;
    private bool isInPlatform;

    private Vector2 ledgePosBot;
    private Vector2 ledgePos1;
    private Vector2 ledgePos2;

    private Rigidbody2D rb;
    private Animator anim;
    private CircleCollider2D cirCollider;
    private BoxCollider2D playerCollider;
    private Collider2D platformsJumpOffCollider;
    private LayerMask playerLayer;


    public bool isGrounded { get; private set; }
    public bool isDashing { get; private set; }

    [Header("Movement")]
    public float movementSpeed = 10.0f;
    [Header("Jumping")]
    [SerializeField] private int amountOfJumps = 1;
    [SerializeField] private float jumpForce = 16.0f;
    [SerializeField] private float airDragMultiplier = 0.95f;
    [SerializeField] private float variableJumpHeightMultiplier = 0.5f;
    [SerializeField] private float movementForceInAir;
    [SerializeField] private float jumpTimerSet = 0.15f;
    [SerializeField] private float groundCheckRadius;
    [Header("Wall Interaction")]
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private float wallSlideSpeed;
    [SerializeField] private float wallHopForce;
    [SerializeField] private float wallJumpForce;
    [SerializeField] private float turnTimerSet = 0.1f;
    [SerializeField] private float wallJumpTimerSet = 0.5f;
    [SerializeField] private Vector2 wallHopDirection;
    [SerializeField] private Vector2 wallJumpDirection;
    [Header("Dashing")]
    [SerializeField] private float dashTime;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float distanceBetweenImages;
    [SerializeField] private float dashCoolDown;
    [Header("Climbing Offsets")]
    [SerializeField] private float ledgeClimbXOffset1 = 0f;
    [SerializeField] private float ledgeClimbYOffset1 = 0f;
    [SerializeField] private float ledgeClimbXOffset2 = 0f;
    [SerializeField] private float ledgeClimbYOffset2 = 0f;
    [Header("Check Objects")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private Transform ledgeCheck;
    [SerializeField] private Transform platformCheck;
    [Header("Knockback")]
    [SerializeField] private float knockbackDuration;
    [SerializeField] private Vector2 knockbackSpeed;
    [Header("Other")]
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private LayerMask whatIsPlatforms;
    [SerializeField] private Vector2 collisionCheckBoxSize;
   

    // Start is called before the first frame update
    void Start()
    {

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        cirCollider = GetComponent<CircleCollider2D>();
        playerCollider = GetComponent<BoxCollider2D>();
        playerLayer = LayerMask.NameToLayer("Player");


        amountOfJumpsLeft = amountOfJumps;
        wallHopDirection.Normalize();
        wallJumpDirection.Normalize();
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
        CheckMovementDirection();
        UpdateAnimations();
        CheckIfCanJump();
        CheckIfWallSliding();
        CheckJump();
        CheckLedgeClimb();
        CheckDash();
        CheckKnockback();
    }

    private void FixedUpdate()
    {
        ApplyMovement();
        CheckSurroundings();
    }

    private void UpdateAnimations()
    {
        anim.SetBool("isWalking", isWalking);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("yVelocity", rb.velocity.y);
        anim.SetBool("isWallSliding", isWallSliding);
    }

    //Inputs and Movement
    private void CheckInput()
    {
        //Movement
        movementInputDirection = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump") && !Input.GetKey(KeyCode.S))
        {
            if (isGrounded || (amountOfJumpsLeft > 0 && !isTouchingWall))
            {
                NormalJump();
            }
            else
            {
                jumpTimer = jumpTimerSet;
                isAttemptingToJump = true;
            }
        }
        //jump off
        else if (Input.GetButtonDown("Jump") && Input.GetKey(KeyCode.S))
        {
            StartCoroutine("JumpOff");
        }

        if (Input.GetButtonDown("Horizontal") && isTouchingWall)
        {
            if (!isGrounded && movementInputDirection != facingDirection)
            {
                canMove = false;
                canFlip = false;

                turnTimer = turnTimerSet;
            }
        }

        if (turnTimer >= 0)
        {
            turnTimer -= Time.deltaTime;

            if (turnTimer <= 0 )
            {
                canMove = true;
                canFlip = true;
            }
        }

        if (checkJumpMultiplier && !Input.GetButton("Jump"))
        {
            checkJumpMultiplier = false;
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * variableJumpHeightMultiplier);
        }

        if (Input.GetButtonDown("Dash") && canDash)
        {
            //check for DashCoolDown
            if (lastDash + dashCoolDown <= Time.time)
                AttemptToDash();
        }

    }

    private void ApplyMovement()
    {

        if (!isGrounded && !isWallSliding && movementInputDirection == 0 && !knockback)
        {
            rb.velocity = new Vector2(rb.velocity.x * airDragMultiplier, rb.velocity.y);
        }
        else if (canMove && !knockback)
        {
            rb.velocity = new Vector2(movementSpeed * movementInputDirection, rb.velocity.y);
        }


        if (isWallSliding)
        {
            if (rb.velocity.y < -wallSlideSpeed)
            {
                rb.velocity = new Vector2(rb.velocity.x, -wallSlideSpeed);
            }
        }
    }

    private void CheckSurroundings()
    {

        if(Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround) ||
        Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsPlatforms))
        {
            
            isGrounded = true;
        }
       else
        {
            isGrounded = false;
        }
            
        isInPlatform = Physics2D.OverlapBox(wallCheck.position, collisionCheckBoxSize, 0, whatIsPlatforms);
            
        isTouchingWall = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance, whatIsGround);
        isTouchingLedge = Physics2D.Raycast(ledgeCheck.position, transform.right, wallCheckDistance, whatIsGround);
        

        if (isTouchingWall && !isTouchingLedge && !ledgeDetected && !isInPlatform)
        {
            ledgeDetected = true;
            ledgePosBot = ledgeCheck.position;
        }
    }

    private void CheckMovementDirection()
    {
        if (isFacingRight && movementInputDirection < 0)
        {
            Flip();
        }
        else if (!isFacingRight && movementInputDirection > 0)
        {
            Flip();
        }

        if (Mathf.Abs(rb.velocity.x) >= 0.01f)
        {
            isWalking = true;
        }
        else
        {
            isWalking = false;
        }
    }

    //Knockback
    public void Knockback(int direction)
    {
        knockback = true;
        knockbackStartTime = Time.time;
        rb.velocity = new Vector2(knockbackSpeed.x * direction, knockbackSpeed.y);
    }

    private void CheckKnockback()
    {
        if (Time.time >= knockbackStartTime + knockbackDuration && knockback)
        {
            knockback = false;
            rb.velocity = new Vector2(0.0f, rb.velocity.y);
        }
    }

    //Wall Sliding
    private void CheckIfWallSliding()
    {
        if (!isGrounded 
            && isTouchingWall 
            && movementInputDirection == facingDirection 
            && rb.velocity.y <= 0 
            && !canClimbLedge)
        {
            isWallSliding = true;
        }
        else
        {
            isWallSliding = false;
        }
    }

    //Ledge Climbing
    private void CheckLedgeClimb()
    {
        if (canClimb)
        {

            if (ledgeDetected && !canClimbLedge && !isGrounded)
            {
                canClimbLedge = true;

                if (isFacingRight)
                {
                    ledgePos1 = new Vector2(Mathf.Floor(ledgePosBot.x + wallCheckDistance) - ledgeClimbXOffset1, Mathf.Floor(ledgePosBot.y) + ledgeClimbYOffset1);
                    ledgePos2 = new Vector2(Mathf.Floor(ledgePosBot.x + wallCheckDistance) + ledgeClimbXOffset2, Mathf.Floor(ledgePosBot.y) + ledgeClimbYOffset2);
                }
                else
                {
                    ledgePos1 = new Vector2(Mathf.Ceil(ledgePosBot.x - wallCheckDistance) + ledgeClimbXOffset1, Mathf.Floor(ledgePosBot.y) + ledgeClimbYOffset1);
                    ledgePos2 = new Vector2(Mathf.Ceil(ledgePosBot.x - wallCheckDistance) - ledgeClimbXOffset2, Mathf.Floor(ledgePosBot.y) + ledgeClimbYOffset2);
                }
                canMove = false;
                canFlip = false;
                canDash = false;


                anim.SetBool("canClimbLedge", canClimbLedge);
            }

            if (canClimbLedge)
            {
                transform.position = ledgePos1;
            }

        }

    }

    public void FinishLedgeClimb()
    {
        canClimbLedge = false;
        transform.position = ledgePos2;
        ledgeDetected = false;
        rb.velocity = new Vector2(rb.velocity.x, 0f);
        anim.SetBool("canClimbLedge", canClimbLedge);
        canMove = true;
        canFlip = true;
        canDash = true;
        isGrounded = true;
    }

    //Dashing
    private void AttemptToDash()
    {
        isDashing = true;
        dashTimeLeft = dashTime;
        lastDash = Time.time;

        PlayerAfterImagePool.Instance.GetFromPool();
        lastImageXPos = transform.position.x;
    }

    private void CheckDash()
    {
        if (isDashing)
        {
            if (dashTimeLeft > 0)
            {
                canMove = false;
                canFlip = false;

                rb.velocity = new Vector2(dashSpeed * facingDirection, rb.velocity.y);
                dashTimeLeft -= Time.deltaTime;

                if (Mathf.Abs(transform.position.x - lastImageXPos) > distanceBetweenImages)
                {
                    PlayerAfterImagePool.Instance.GetFromPool();
                    lastImageXPos = transform.position.x;
                }
            }

            if (dashTimeLeft <= 0 || isTouchingWall)
            {
                isDashing = false;
                canMove = true;
                canFlip = true;
                canDash = true;
            }

        }
    }

    //Jumping
    private void CheckJump()
    {
        //jump off
        
            if (rb.velocity.y > 0)
            {
                RaycastHit2D hit = Physics2D.Raycast(platformCheck.position, Vector3.up, 0.5f);
                if (hit)
                {
                        if (hit.collider.CompareTag("JumpOffPlatform"))
                    {
                        Physics2D.IgnoreLayerCollision(playerLayer, 13, true);
                        canClimb = false;
                    }
                        
                }
            }
            else if (rb.velocity.y <= 0 && !isInPlatform && !jumpOffCoroutineIsRunning)
            {
                Physics2D.IgnoreLayerCollision(playerLayer, 13, false);
                canClimb = true;
            }
        

        if (jumpTimer > 0)
        {
            //WallJump
            if (!isGrounded && isTouchingWall && movementInputDirection != 0 && movementInputDirection != facingDirection)
            {
                WallJump();
            }
            else if (isGrounded)
            {
                NormalJump();
            }
        }

        if (isAttemptingToJump)
        {
            jumpTimer -= Time.deltaTime;
        }

        if (wallJumpTimer > 0)
        {
            if (hasWallJumped && movementInputDirection == -lastWallJumpDirection)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0.0f);
                hasWallJumped = false;
            }
            else if (wallJumpTimer <= 0)
            {
                hasWallJumped = false;
            }
            else
            {
                wallJumpTimer -= Time.deltaTime;
            }
        }
    }

    private void NormalJump()
    {
        if (canNormalJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            amountOfJumpsLeft--;
            jumpTimer = 0;
            isAttemptingToJump = false;
            checkJumpMultiplier = true;
        }
    }

    private void WallJump()
    {
        if (canWallJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0.0f);
            isWallSliding = false;
            amountOfJumpsLeft = amountOfJumps;
            amountOfJumpsLeft--;
            Vector2 forceToAdd = new Vector2(wallJumpForce * wallJumpDirection.x * movementInputDirection, wallJumpForce * wallJumpDirection.y);
            rb.AddForce(forceToAdd, ForceMode2D.Impulse);
            jumpTimer = 0;
            isAttemptingToJump = false;
            checkJumpMultiplier = true;
            turnTimer = 0;
            canMove = true;
            canFlip = true;
            hasWallJumped = true;
            wallJumpTimer = wallJumpTimerSet;
            lastWallJumpDirection = -facingDirection;

        }
    }

    private void CheckIfCanJump()
    {
        if (isGrounded && rb.velocity.y <= 0.01f)
        {
            amountOfJumpsLeft = amountOfJumps;
        }

        if (isTouchingWall)
        {
            checkJumpMultiplier = false;
            canWallJump = true;
        }

        if (amountOfJumpsLeft <= 0)
        {
            canNormalJump = false;
        }
        else
        {
            canNormalJump = true;
        }

    }
    //jump off
    IEnumerator JumpOff()
    {
            jumpOffCoroutineIsRunning = true;
            Collider2D collider = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsPlatforms);
            if (collider)
            {
                if (collider.CompareTag("JumpOffPlatform"))
                {
                    canClimb = false;
                    Physics2D.IgnoreLayerCollision(playerLayer, 13, true);
                    yield return new WaitForSeconds(0.1f);

                    yield return new WaitUntil(() => !isInPlatform);
                    
                    Physics2D.IgnoreLayerCollision(playerLayer, 13, false);
                    canClimb = true;
                    jumpOffCoroutineIsRunning = false;
                }
            }
            else
            {
                jumpOffCoroutineIsRunning = false;
            }
    }

    //Flipping
    public void DisableFlip()
    {
        canFlip = false;
    }

    public void EnableFlip()
    {
        canFlip = true;
    }

    private void Flip()
    {
        if (!isWallSliding && canFlip && !knockback)
        {
            facingDirection *= -1;
            isFacingRight = !isFacingRight;
            transform.Rotate(0.0f, 180.0f, 0.0f);
        }
    }

    //Gizmos
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);

        Gizmos.DrawLine(ledgePos1, ledgePos2);

        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y, wallCheck.position.z));

        Gizmos.DrawLine(ledgeCheck.position, new Vector3(ledgeCheck.position.x + wallCheckDistance, ledgeCheck.position.y, ledgeCheck.position.z));

        Gizmos.DrawCube(wallCheck.position, new Vector3(collisionCheckBoxSize.x, collisionCheckBoxSize.y, 0f));
   
    }

    //------------------GETTERS-------------------------

    public int GetFacingDirection()
    {
        return facingDirection;
    }

    public bool GetDashStatus()
    {
        return isDashing;
    }
}
