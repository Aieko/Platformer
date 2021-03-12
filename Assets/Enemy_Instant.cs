using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Instant : MonoBehaviour
{
    public Animator animator;
   
    public int maxHealth = 100;
    int currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }


    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        //play hurt animation

        animator.SetTrigger("Hurt");

        if (currentHealth <= 0 )
        {
            Die();
        }
    }

    void Die()
    {
        animator.SetBool("IsDead", true);
        this.GetComponent<Collider2D>().enabled = false;
    }
   
}
