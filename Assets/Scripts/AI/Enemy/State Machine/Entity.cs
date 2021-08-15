using UnityEngine;

public class Entity : MonoBehaviour, IDamageable
{
    
    [Header("Base Entity")]
    public D_Entity entityData;


    #region Components
    public Animator anim { get; private set; }

    public AnimationToStateMachine atsm { get; private set; }

    public Core Core { get; private set; }

    public FiniteStateMachine stateMachine { get; private set; }

    #endregion

    public bool canBeHurt { get; private set; }

    public float lastHitTime { get; private set; }

    public bool isGetHit { get; protected set; }

    public int lastHitDirection { get; private set; }


    [Header("Checks")]
    [SerializeField]
    private Transform playerCheck;

    private float currentHealth;
    private float currentStunResistance;
    private float lastDamageTime;
    //Layers to Ignore
    private int ignoreLayers;

    protected bool isStunned;
    protected bool isDead;


    private Vector2 velocityWorkspace;

    [Header("UI")]
    [SerializeField]
    private EnemyHealthBar_Script HealthBar;

    public virtual void Awake()
    {

        anim = GetComponent<Animator>();
        atsm = GetComponent<AnimationToStateMachine>();
        Core = GetComponentInChildren<Core>();

        isGetHit = false;
        currentHealth = entityData.maxHealth;
        currentStunResistance = entityData.stunResistance;
        canBeHurt = entityData.canBeHurt;
        //bitwise operation for ignoring multiple layers
        ignoreLayers = ~( (1 << LayerMask.NameToLayer("Damageable")) | (1 << LayerMask.NameToLayer("Secret")) ); 

        stateMachine = new FiniteStateMachine();

        HealthBar.SetMaxHealth(entityData.maxHealth);
   
    }

    public virtual void Update()
    {
        Core.LogicUpdate();

        stateMachine.currentState.LogicUpdate();

        anim.SetFloat("YVelocity", Core.Movement.RB.velocity.y);

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
 
    public virtual bool CheckPlayerInSight()
    {
        var hit = Physics2D.Linecast(playerCheck.position, playerCheck.position + (Vector3)(Vector2.right * entityData.maxAgroDistance * Core.Movement.FacingDirection), ignoreLayers);

        return hit.collider != null && hit.collider.CompareTag("Player");
    }

    public virtual bool CheckPlayerInMinAgroRange()
    {
        return Physics2D.Raycast(playerCheck.position, transform.right, entityData.minAgroDistance, entityData.whatIsPlayer);
    }

    public virtual bool CheckPlayerInMaxAgroRange()
    {
        return Physics2D.Raycast(playerCheck.position,  transform.right, entityData.maxAgroDistance, entityData.whatIsPlayer);
    }

    public virtual bool CheckPlayerInCloseRangeAction()
    {
        return Physics2D.Raycast(playerCheck.position, transform.right, entityData.closeRangeActionDistance, entityData.whatIsPlayer);
    }

    //--OTHER FUNCTIONS-------------------------------------------------------------------------------

    public virtual void Damage(AttackDetails attackDetails)
    {
        if (attackDetails.position.x > transform.position.x)
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
        
        Instantiate(entityData.hitParticle, transform.position, Quaternion.Euler(0f, 0f, Random.Range(0f, 360f)));

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
            gameObject.SetActive(false);
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
        velocityWorkspace.Set(Core.Movement.RB.velocity.x, velocityY);
        Core.Movement.RB.velocity = velocityWorkspace;
    }

  

    public virtual void OnDrawGizmos()
    {
        /*
        if(Core)
        {
             Gizmos.DrawLine(wallCheck.position, wallCheck.position + (Vector3)(Vector2.right * Core.Movement.FacingDirection * entityData.wallCheckDistance));
            Gizmos.DrawLine(ledgeCheck.position, ledgeCheck.position + (Vector3)(Vector2.down * entityData.ledgeCheckDistance));
            Gizmos.DrawLine(playerCheck.position + (Vector3)(Vector2.right * Core.Movement.FacingDirection), playerCheck.position + (Vector3)(Vector2.right * Core.Movement.FacingDirection * entityData.maxAgroDistance));

             Gizmos.DrawWireSphere(playerCheck.position + (Vector3)(Vector2.right * entityData.closeRangeActionDistance * Core.Movement.FacingDirection), 0.2f);

             Gizmos.DrawWireSphere(playerCheck.position + (Vector3)(Vector2.right * entityData.minAgroDistance * Core.Movement.FacingDirection), 0.2f);
            Gizmos.DrawWireSphere(playerCheck.position + (Vector3)(Vector2.right * entityData.maxAgroDistance * Core.Movement.FacingDirection), 0.2f);
        }*/
    }
}
