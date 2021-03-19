using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Instant : MonoBehaviour
{
    


    public int maxHealth = 3;
    int currentHealth;

    bool IsDead = false;
    
    public EnemyHealthBar_Script EnemyhealthBar;
    EnemyAI enemyAI;
    Rigidbody2D rb;
    public Animator animator;
    float horizontalMove;

    //public float attackRange = 0.5f;
    public int attackDamage = 1;

    public float attackRate = 0.5f;
    float nextAttackTime = 0f;

    public Vector2 pushingforce = new Vector2(180f,0f);

    private bool m_FacingRight = false;  // For determining which way the player is currently facing.

    enum enemyState { Idle, CombatIdle, Run}

    bool IsPatrol = false;
    
    public bool GetFlip()
    {
        return m_FacingRight;
    }
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        EnemyhealthBar.SetMaxHealth(maxHealth);
        enemyAI = GetComponent<EnemyAI>();
        rb = GetComponent<Rigidbody2D>();      

    }

    void FixedUpdate()
    {
        float distanceToPlayer = Vector2.Distance(rb.position, enemyAI.player.transform.position);

        if (enemyAI.IsChasing)
        {
            if (enemyAI.player.transform.position.x > transform.position.x && !m_FacingRight)
            {
                Flip();
            }
            else if (enemyAI.player.transform.position.x < transform.position.x && m_FacingRight)
            {
                Flip();
            }
        }
        else if(IsPatrol)
        {
            if (enemyAI.path.vectorPath[0].x > transform.position.x && !m_FacingRight)
            {
                Flip();
            }
            else if (enemyAI.path.vectorPath[0].x < transform.position.x && m_FacingRight)
            {
                Flip();
            }
        }
       

        if (enemyAI.IsChasing && distanceToPlayer <= 1f && !animator.GetCurrentAnimatorStateInfo(0).IsName("Hurt"))
        {
            if (Time.time >= nextAttackTime)
            {
                
                Attack();
                
                nextAttackTime = Time.time + 1f / attackRate;
            }
            
        }

        if (Mathf.Abs(rb.velocity.x) > 1f)
        {
            animator.SetInteger("AnimState", 2);
        }
        else 
        {
            if (Mathf.Abs(rb.velocity.x) < 0.8f && enemyAI.IsChasing)
            {
                animator.SetInteger("AnimState", 1);
            }

            else if (Mathf.Abs(rb.velocity.x)< 0.8f)
            {
                animator.SetInteger("AnimState", 0);
            } 
        }


    }
    public void TakeDamage(int damage)
    {
        if(!IsDead)
        currentHealth -= damage;

        //play hurt animation

        if (m_FacingRight)
        {
            rb.AddForce(-pushingforce, ForceMode2D.Impulse);
        }
        else
            rb.AddForce(pushingforce, ForceMode2D.Impulse);

        animator.SetTrigger("Hurt");

        EnemyhealthBar.SetHealth(currentHealth);

        if (currentHealth <= 0 )
        {
            Die();
        }
    }

    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        m_FacingRight = !m_FacingRight;
        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;

    }

    public void Attack()
    {
        
        animator.SetTrigger("Attack");
        float AttackTimeAnim = animator.recorderStopTime;

    }
    void Die()
    {
        animator.SetBool("IsDead", true);
        IsDead = true;

        this.GetComponent<CircleCollider2D>().enabled = false;
        this.GetComponent<BoxCollider2D>().enabled = false;
        this.enabled = false;
        enemyAI.enabled = false;
        enemyAI.IsChasing = false;
        EnemyhealthBar.EnableHealthBar(false);
        rb.simulated = false;
        

    }
   
}
