using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Instant : MonoBehaviour
{
    public Animator animator;
   
    public int maxHealth = 100;
    int currentHealth;

    public EnemyHealthBar_Script EnemyhealthBar;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        EnemyhealthBar.SetMaxHealth(maxHealth);
    }


    public void TakeDamage(int damage)
    {
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

        this.GetComponent<Collider2D>().enabled = false;

        this.enabled = false;

        EnemyhealthBar.EnableHealthBar(false);
    }
   
}
