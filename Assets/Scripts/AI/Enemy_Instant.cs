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
    Rigidbody2D rg;
    

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        EnemyhealthBar.SetMaxHealth(maxHealth);
        enemyAI = GetComponent<EnemyAI>();
        rg = GetComponent<Rigidbody2D>();
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
        rg.simulated = false;
        

    }
   
}
