using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour
{
	[SerializeField] private float m_JumpForce = 400f;							// Amount of force added when the player jumps.
	[Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;			// Amount of maxSpeed applied to crouching movement. 1 = 100%
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;	// How much to smooth out the movement
	[SerializeField] private bool m_AirControl = false;							// Whether or not a player can steer while jumping;
	[SerializeField] private LayerMask m_WhatIsGround;							// A mask determining what is ground to the character
	[SerializeField] private Transform m_GroundCheck;							// A position marking where to check if the player is grounded.
	[SerializeField] private Transform m_CeilingCheck;							// A position marking where to check for ceilings
    [SerializeField] private Transform m_WallCheck;
    [SerializeField] private Collider2D m_CrouchDisableCollider;				// A collider that will be disabled when crouching
    [SerializeField] private int amountOfJumps = 1;

    const float k_GroundedRadius = .1f; // Radius of the overlap circle to determine if grounded
	public bool m_Grounded;            // Whether or not the player is grounded.
    public float wallCheckDistance;
    public bool m_FacingRight = true;  // For determining which way the player is currently facing.
    public float wallSlidingSpeed;
    const float k_CeilingRadius = .2f; // Radius of the overlap circle to determine if the player can stand up

	private Rigidbody2D m_Rigidbody2D;
	private Vector3 m_Velocity = Vector3.zero;
    private int amountOfJumpsLeft;
    private bool canJump;
    private bool isWallSliding;
    private bool isTouchingWall;
   

	[Header("Events")]
	[Space]

	public UnityEvent OnLandEvent;

	[System.Serializable]
	public class BoolEvent : UnityEvent<bool> { }

	public BoolEvent OnCrouchEvent;
	private bool m_wasCrouching = false;

	private void Awake()
	{
        
		m_Rigidbody2D = GetComponent<Rigidbody2D>();

		if (OnLandEvent == null)
			OnLandEvent = new UnityEvent();

		if (OnCrouchEvent == null)
			OnCrouchEvent = new BoolEvent();

        amountOfJumpsLeft = amountOfJumps;


	}

    private void Update()
    {
        CheckIfCanJump();
        CheckIfWallSliding();
    }

    private void FixedUpdate()
	{
		bool wasGrounded = m_Grounded;
		m_Grounded = false;

        //Checking is player gorunded or not
        CheckSurroundings(wasGrounded);
		
	}
    private void CheckIfWallSliding()
    {
        if (isTouchingWall
            && !m_Grounded
            && m_Rigidbody2D.velocity.y < 0)
        {
            isWallSliding = true;
            amountOfJumpsLeft = amountOfJumps;

        }
        else
            isWallSliding = false;

    }
    private void CheckSurroundings(bool wasGrounded)
    {
        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                m_Grounded = true;
                if (!wasGrounded)
                    OnLandEvent.Invoke();
            }
        }
        
       
        isTouchingWall = Physics2D.Raycast(m_WallCheck.position, transform.right, wallCheckDistance, m_WhatIsGround);
       
    }



	public void Move(float move, bool crouch, bool jump)
	{
		// If crouching, check to see if the character can stand up
		if (!crouch)
		{
			// If the character has a ceiling preventing them from standing up, keep them crouching
			if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
			{
				crouch = true;
			}
		}

        //If wallSliding
        if(isWallSliding)
        {
            if(m_Rigidbody2D.velocity.y < -wallSlidingSpeed)
            {
                m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, -wallSlidingSpeed);
            }
            
        }

		//only control the player if grounded or airControl is turned on
		if (m_Grounded || m_AirControl)
		{

            // If crouching
            OnCrouch(move, crouch);

            // Move the character by finding the target velocity
            Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
			// And then smoothing it out and applying it to the character
			m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

			// If the input is moving the player right and the player is facing left...
			if (move > 0 && !m_FacingRight)
			{
				// ... flip the player.
				Flip();
			}
			// Otherwise if the input is moving the player left and the player is facing right...
			else if (move < 0 && m_FacingRight)
			{
				// ... flip the player.
				Flip();
			}
		}
        // If the player should jump...
        OnJump(jump);

    }

    private void OnCrouch(float move, bool crouch)
    {
        if (crouch)
        {
            if (!m_wasCrouching)
            {
                m_wasCrouching = true;
                OnCrouchEvent.Invoke(true);
            }

            // Reduce the speed by the crouchSpeed multiplier
            move *= m_CrouchSpeed;

            // Disable one of the colliders when crouching
            if (m_CrouchDisableCollider != null)
                m_CrouchDisableCollider.enabled = false;
        }
        else
        {
            // Enable the collider when not crouching
            if (m_CrouchDisableCollider != null)
                m_CrouchDisableCollider.enabled = true;

            if (m_wasCrouching)
            {
                m_wasCrouching = false;
                OnCrouchEvent.Invoke(false);
            }
        }
    }

    private void OnJump(bool isJumping)
    {
        if (isJumping
            && canJump)
        {
            m_Grounded = true;
            // Add a vertical force to the player.
                amountOfJumpsLeft--;
            m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, m_JumpForce);
        }
    }

   private void CheckIfCanJump()
    {
        if (m_Grounded && m_Rigidbody2D.velocity.y <= 0)

        amountOfJumpsLeft = amountOfJumps;

        if (amountOfJumpsLeft <= 0)
        {
            canJump = false;
        }
        else canJump = true;
    }
    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        m_FacingRight = !m_FacingRight;
        transform.Rotate(0f, 180f, 0f);
      

    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(m_WallCheck.position, new Vector3(m_WallCheck.position.x + wallCheckDistance, m_WallCheck.position.y, m_WallCheck.position.z));
    }
}
