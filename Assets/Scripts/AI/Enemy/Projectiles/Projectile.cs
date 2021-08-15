using UnityEngine;

public class Projectile : MonoBehaviour
{
    private AttackDetails attackDetails;

    private float speed;
    private float travelDistance;
    private float xStartPos;
    private float explodeTime;
    private float startExplodingTime;

    private Rigidbody2D rb;
    private Animator anim;
    private bool isGravityOn;
    private bool hasHitGround;
    private bool explode = false;

    [SerializeField] private bool shouldExplode;
    [SerializeField] private bool shouldExplOnHit;
    [SerializeField] private bool shouldDamOnHit;
    [SerializeField] private float explosionRadius;
    [SerializeField] private bool shouldStickToWalls;
    [SerializeField] private float gravity;
    [SerializeField] private float damageRadius;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private LayerMask whatIsPlayer;
    [SerializeField] private Transform damagePosition;
    

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        rb.gravityScale = 0.0f;

        rb.velocity = transform.right * speed;

        isGravityOn = false;
        xStartPos = transform.position.x;

        Invoke("Destroy", 10f);
    }

    private void Update()
    {
        if (hasHitGround) return;
        
        attackDetails.position = transform.position;

            if(isGravityOn)
            {
                var angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;

                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
        
    }
    //TODO change way of dealing damage to player
    private void FixedUpdate()
    {

        if (hasHitGround) return;
        
        var damageHit = Physics2D.OverlapCircle(damagePosition.position, damageRadius, whatIsPlayer);
        var groundHit = Physics2D.OverlapCircle(damagePosition.position, damageRadius, whatIsGround);

            if(damageHit)
            {

                Debug.Log("You were hit by projectile!");

                var player = damageHit.transform.gameObject.GetComponent<Player>();

                if (player.StateMachine.currentState == player.DashState) return;
                
                if(shouldDamOnHit || shouldExplOnHit)
                {
                    if (shouldDamOnHit)
                    {
                        player.Damage(10f);

                        Destroy(gameObject);
                    }

                    if (shouldExplOnHit && !explode)
                    {
                        Explosion();
                    } 
                }    
            }

            if(groundHit)
            {
                hasHitGround = true;

                if (shouldExplOnHit && !explode)
                {
                    Explosion();
                }

                if (shouldStickToWalls)
                {
                    rb.gravityScale = 0f;
                    rb.velocity = Vector2.zero;
                }  
            }

            if (Mathf.Abs(xStartPos - transform.position.x) >= travelDistance && !isGravityOn)
            {
                isGravityOn = true;

                rb.gravityScale = gravity;
            }
          
    }

    public void Explosion()
    {
        

        rb.gravityScale = 0f;
        rb.velocity = Vector2.zero;
        

        Debug.Log("Boom");
          
           var explosionHit = Physics2D.OverlapCircle(damagePosition.position, explosionRadius, whatIsPlayer);

           if (explosionHit)
           {
               var player = explosionHit.transform.gameObject.GetComponent<Player>();

               if (player.StateMachine.currentState == player.DashState) return;

               player.Damage(10f);

               int direction;

               if (player.transform.position.x < transform.position.x)
               {
                   direction = -1;
               }
               else
               {
                   direction = 1;
               }

            player.Core.Movement.SetVelocity(20f, new Vector2(3f,2f), direction);
           }

        explode = true;
        anim.SetBool("Boom", true);
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    public void FireProjectile(float speed, float travelDistance, float damage)
    {
        this.speed = speed;
        this.travelDistance = travelDistance;
        attackDetails.damageAmount = damage;
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(damagePosition.position, damageRadius);
    }
}
