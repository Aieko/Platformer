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
    float ResetComboTime = 1f;
    int Combo = 0;

    void Update()
    {
        ResetComboTime -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Mouse0) && Time.time >=nextAttackTime)
        {
            

            //Attack Animation
            if(Combo == 0)
            {
                animator.SetTrigger("Attack1");
                Attack();
                nextAttackTime = Time.time + 1f / attackRate;
                Combo++;
                InvokeRepeating("ResetCombo", 1f,0f);
            }
            else if(Combo == 1)
            {
                CancelInvoke("ResetCombo");
                animator.SetTrigger("Attack2");
                Attack();
                nextAttackTime = Time.time + 1f / attackRate;
                ResetCombo();
               
            }

        }

    }

   void Attack()
    {
        

        //Detect enemies in range of attack
       Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach(Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy_Instant>().TakeDamage(attackDamage);
            //enemy.GetComponent<Rigidbody2D>().AddForce();
        }
    }

    void ResetCombo()
    {
        Combo = 0;
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


