using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeCombat : MonoBehaviour
{
    public Animator animator;

    public Transform attackPoint;
    public LayerMask enemyLayers;
    public Enemy_Instant enemy;

    public float attackRange = 0.5f;
    public int attackDamage = 40;

    public float attackRate = 2f;
    float nextAttackTime = 0f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && Time.time >=nextAttackTime)
        {
            Attack();
            nextAttackTime = Time.time + 1f / attackRate;
        }

    }

   void Attack()
    {
        //Attack Animation
        animator.SetTrigger("Attack");

        //Detect enemies in range of attack
       Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach(Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy_Instant>().TakeDamage(attackDamage);
        }
    }

    void OnDrawGizmosSelected()
    {
        if(attackPoint == null)
         return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        Physics2D.IgnoreCollision(enemy.GetComponent<CircleCollider2D>(), GetComponent<Collider2D>());
        Physics2D.IgnoreCollision(enemy.GetComponent<CircleCollider2D>(), GetComponent<Collider2D>());
    }
}


