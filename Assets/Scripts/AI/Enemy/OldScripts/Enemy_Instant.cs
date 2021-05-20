using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Instant : MonoBehaviour
{
    
    public int maxHealth = 3;
    int currentHealth;
    public EnemyHealthBar_Script EnemyhealthBar;
    public GameObject player;

    bool IsDead = false;
    
    EnemyAI enemyAI;
    Rigidbody2D rb;
    public Animator animator;

    //public float attackRange = 0.5f;
    public int attackDamage = 1;
    

    public Vector2 pushingforce = new Vector2(180f,0f); // power of pushing when gets hit

   

   
    
   
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
        if (Mathf.Abs(rb.velocity.x) > 0.2f)
        {
            animator.SetInteger("AnimState", 2);
        }
        else 
        {
            if (Mathf.Abs(rb.velocity.x) < 0.2f && enemyAI.IsChasing)
            {
                animator.SetInteger("AnimState", 1);
            }

            else if (Mathf.Abs(rb.velocity.x)< 0.2f)
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

        if (enemyAI.getFacingDir()) // true = facing right
        {
            rb.AddForce(-pushingforce, ForceMode2D.Impulse);
        }
        else
        {
            rb.AddForce(pushingforce, ForceMode2D.Impulse);
        }
            

        animator.SetTrigger("Hurt");

        EnemyhealthBar.SetHealth(currentHealth);

        if (currentHealth <= 0 )
        {
            Die();
        }
    }

    

    public virtual void Attack()
    {
        
        animator.SetTrigger("Attack");
       

    }

    public virtual void Jump()
    {

    }
    void Die()
    {
        animator.SetBool("IsDead", true);
        IsDead = true;

        
        this.GetComponent<BoxCollider2D>().enabled = false;
        this.enabled = false;
        enemyAI.enabled = false;
        EnemyhealthBar.EnableHealthBar(false);

        this.GetComponent<CircleCollider2D>().enabled = false;
        rb.simulated = false;
        

    }
   
}
