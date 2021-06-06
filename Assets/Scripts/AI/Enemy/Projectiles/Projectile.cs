using System.Collections;
using System.Collections.Generic;
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
    }

    private void Update()
    {
        if(!hasHitGround)
        {
            attackDetails.position = transform.position;
            if(isGravityOn)
            {
                float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;

                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
        }
    }

    private void FixedUpdate()
    {
       
        if(!hasHitGround)
        {
            Collider2D damageHit = Physics2D.OverlapCircle(damagePosition.position, damageRadius, whatIsPlayer);
            Collider2D groundHit = Physics2D.OverlapCircle(damagePosition.position, damageRadius, whatIsGround);

            if(damageHit)
            {
                CharacterController2D PC = damageHit.transform.gameObject.GetComponent<CharacterController2D>();

                if (PC.isDashing)
                    return;

                else if(shouldDamOnHit || shouldExplOnHit)
                {
                    if (shouldDamOnHit)
                    {
                        damageHit.transform.SendMessage("Damage", attackDetails);
                    }

                    if (shouldExplOnHit)
                    {
                        Explosion();
                    }
                    

                    Destroy(gameObject);
                }           
            }

            if(groundHit)
            {
                hasHitGround = true;

                if(shouldStickToWalls)
                {
                    rb.gravityScale = 0f;
                    rb.velocity = Vector2.zero;

                    Invoke("Destroy", 10f);
                }
               
                
            }
            if (Mathf.Abs(xStartPos - transform.position.x) >= travelDistance && !isGravityOn)
            {
                isGravityOn = true;

                rb.gravityScale = gravity;
            }
        }

       

       
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }

    public void Explosion()
    {
        rb.gravityScale = 0f;
        rb.velocity = Vector2.zero;

        Collider2D explosionHit = Physics2D.OverlapCircle(damagePosition.position, explosionRadius, whatIsPlayer);
        if (explosionHit)
        {
            CharacterController2D PC = explosionHit.transform.gameObject.GetComponent<CharacterController2D>();

            if (PC.isDashing)
                return;
            else
            {
                explosionHit.transform.SendMessage("Damage", attackDetails);
               
                Destroy(gameObject);
            }


        }

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
