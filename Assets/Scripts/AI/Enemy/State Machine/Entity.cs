using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public FiniteStateMachine stateMachine;
    [Header("Base Entity")]
    public D_Entity entityData;

    public int facingDirection { get; private set; }

    public int lastHitDirection { get; private set; }

    public Rigidbody2D rb { get; private set; }

    public Animator anim { get; private set; }

    public GameObject aliveGO { get; private set; }

    public AnimationToStateMachine atsm { get; private set; }

    public bool canBeHurt { get; private set; }

    public float lastHitTime { get; private set; }

    public bool isGetHit { get; protected set; }

    [Header("Checks")]
    [SerializeField]
    private Transform wallCheck;
    [SerializeField]
    private Transform ledgeCheck;
    [SerializeField]
    private Transform playerCheck;
    [SerializeField]
    private Transform groundCheck;

    private float currentHealth;
    private float currentStunResistance;
    private float lastDamageTime;
    //layer mask for player investigation 
    private int friendyLayer;

    protected bool isStunned;
    protected bool isDead;


    private Vector2 velocityWorkspace;

    [Header("UI")]
    [SerializeField]
    private EnemyHealthBar_Script HealthBar;

    private void Awake()
    {
      
    }

    public virtual void Start()
    {
        isGetHit = false;
        facingDirection = 1;
        currentHealth = entityData.maxHealth;
        currentStunResistance = entityData.stunResistance;
        canBeHurt = entityData.canBeHurt;
        friendyLayer = 1 << LayerMask.NameToLayer("Damageable");
        friendyLayer = ~friendyLayer;
        aliveGO = transform.Find("Alive").gameObject;

        rb = aliveGO.GetComponent<Rigidbody2D>();
        anim = aliveGO.GetComponent<Animator>();
        atsm = aliveGO.GetComponent<AnimationToStateMachine>();

        stateMachine = new FiniteStateMachine();

    
        HealthBar.SetMaxHealth(entityData.maxHealth);
   

    }

    public virtual void Update()
    {
        stateMachine.currentState.LogicUpdate();

        anim.SetFloat("YVelocity", rb.velocity.y);

        if(Time.time >= lastDamageTime + entityData.stunRecoveryTime)
        {
            ResetStunResistance();
        }
    }

    public virtual void FixedUpdate()
    {
        stateMachine.currentState.PhysicsUpdate();
    }

    //--CHECKING FUNCTIONS-----------------------------------------------------------------------
    public virtual bool CheckWall()
    {
        
        return Physics2D.Raycast(wallCheck.position, aliveGO.transform.right, entityData.wallCheckDistance, entityData.whatIsGround);
    }

    public virtual bool CheckPlayerInSight()
    {
        RaycastHit2D hit;

        
        hit = Physics2D.Linecast(playerCheck.position, playerCheck.position + (Vector3)(Vector2.right * entityData.maxAgroDistance * facingDirection), friendyLayer);
        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Player"))
            {
                return true;
            }
            else
                return false;
        }
        else
        return false;

        
    }

    public virtual bool CheckLedge()
    {
        return Physics2D.Raycast(ledgeCheck.position, Vector2.down, entityData.ledgeCheckDistance, entityData.whatIsGround); ;
    }

    public virtual bool CheckPlayerInMinAgroRange()
    {
        return Physics2D.Raycast(playerCheck.position, aliveGO.transform.right, entityData.minAgroDistance, entityData.whatIsPlayer);
    }

    public virtual bool CheckPlayerInMaxAgroRange()
    {
        return Physics2D.Raycast(playerCheck.position,  aliveGO.transform.right, entityData.maxAgroDistance, entityData.whatIsPlayer);
    }

    public virtual bool CheckPlayerInCloseRangeAction()
    {
        return Physics2D.Raycast(playerCheck.position, aliveGO.transform.right, entityData.closeRangeActionDistance, entityData.whatIsPlayer);
    }

    public virtual bool CheckGround()
    {
        return Physics2D.OverlapCircle(groundCheck.position, entityData.groundCheckRadius, entityData.whatIsGround);
    }
    //--OTHER FUNCTIONS-------------------------------------------------------------------------------

    public virtual void Flip()
    {
        facingDirection *= -1;

        aliveGO.transform.Rotate(0f, 180f, 0f);
    }

    public virtual void Damage(AttackDetails attackDetails)
    {
        

        if (attackDetails.position.x > aliveGO.transform.position.x)
        {
            lastHitDirection = -1;
        }
        else
        {
            lastHitDirection = 1;
        }

        lastHitTime = Time.time;

        if (!canBeHurt)
            return;

        lastDamageTime = Time.time; 
        currentHealth -= attackDetails.damageAmount;
        currentStunResistance -= attackDetails.stunDamageAmount;
        
        DamageHop(entityData.damageHopSpeedY);
        
        Instantiate(entityData.hitParticle, aliveGO.transform.position, Quaternion.Euler(0f, 0f, Random.Range(0f, 360f)));

        if (HealthBar)
        {
            HealthBar.SetHealth(currentHealth);
        }

        if(currentStunResistance <= 0)
        {
            isStunned = true;
        }

        if(currentHealth <= 0 )
        {
            aliveGO.SetActive(false);
            HealthBar.EnableHealthBar(false);
            isDead = true;
        }

    }

    public virtual void ResetStunResistance()
    {
        isStunned = false;
        currentStunResistance = entityData.stunResistance;
    }

    public virtual void DamageHop(float velocityY)
    {
        velocityWorkspace.Set(rb.velocity.x, velocityY);
        rb.velocity = velocityWorkspace;
    }

    public virtual void SetVelocity(float velocity, Vector2 angle, int direction)
    {
        angle.Normalize();

        velocityWorkspace.Set(angle.x * velocity * direction, angle.y * velocity);
        rb.velocity = velocityWorkspace;

    }

    public virtual void SetVelocity(float velocity)
    {
        if (velocity == 0)
        {
            velocityWorkspace.Set(velocity, velocity);
        }
        else
        {
            velocityWorkspace.Set(facingDirection * velocity, rb.velocity.y);
        }
        
        
        rb.velocity = velocityWorkspace;
    }

    public virtual void OnDrawGizmos()
    {
       // Gizmos.DrawLine(wallCheck.position, wallCheck.position + (Vector3)(Vector2.right * facingDirection * entityData.wallCheckDistance));
       //Gizmos.DrawLine(ledgeCheck.position, ledgeCheck.position + (Vector3)(Vector2.down * entityData.ledgeCheckDistance));
        Gizmos.DrawLine(playerCheck.position + (Vector3)(Vector2.right * facingDirection), playerCheck.position + (Vector3)(Vector2.right * facingDirection * entityData.maxAgroDistance));

       // Gizmos.DrawWireSphere(playerCheck.position + (Vector3)(Vector2.right * entityData.closeRangeActionDistance * facingDirection), 0.2f);

      //  Gizmos.DrawWireSphere(playerCheck.position + (Vector3)(Vector2.right * entityData.minAgroDistance * facingDirection), 0.2f);
        //Gizmos.DrawWireSphere(playerCheck.position + (Vector3)(Vector2.right * entityData.maxAgroDistance * facingDirection), 0.2f);

    }
}
