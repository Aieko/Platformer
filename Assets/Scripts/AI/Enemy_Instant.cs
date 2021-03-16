using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Instant : MonoBehaviour
{
    public Animator animator;
    
   
    public int maxHealth = 3;
    int currentHealth;

    bool IsDead = false;

    public EnemyHealthBar_Script EnemyhealthBar;
    EnemyAI enemyAI;
    Rigidbody2D rb;
    float horizontalMove;

    //public float attackRange = 0.5f;
    public int attackDamage = 1;

    public float attackRate = 0.5f;
    float nextAttackTime = 0f;

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

        if (enemyAI.IsChasing && distanceToPlayer <= 1f)
        {
            if (Time.time >= nextAttackTime)
            {
                Attack();

                nextAttackTime = Time.time + 1f / attackRate;
            }
            
        }

        if (Mathf.Abs(rb.velocity.x)>0.3)
        {
            animator.SetInteger("AnimState", 2);
        }
        else if (Mathf.Abs(rb.velocity.x) < 0.3)
        {
            if(enemyAI.IsPlayerNearby)
            animator.SetInteger("AnimState", 1);
            else animator.SetInteger("AnimState", 0);
        }


    }
    public void TakeDamage(int damage)
    {
        if(!IsDead)
        currentHealth -= damage;

        //play hurt animation

        animator.SetTrigger("Hurt");

        EnemyhealthBar.SetHealth(currentHealth);

        if (currentHealth <= 0 )
        {
            Die();
        }
    }

    public void Attack()
    {
        animator.SetTrigger("Attack");
        

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
