﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeCombat : MonoBehaviour
{
    public Animator animator;

    public Transform attackPoint;
    public LayerMask enemyLayers;
    public Enemy_Instant m_enemy;

    CharacterController2D controller;

    public float attackRange = 0.5f;
    public int attackDamage = 40;

    public float attackRate = 2f;
    float nextAttackTime = 0f;
   
    int Combo = 0;

    private void Awake()
    {
        controller = GetComponent<CharacterController2D>();
    }

    void Update()
    {
        

        if (Input.GetKeyDown(KeyCode.Mouse0) && Time.time >=nextAttackTime
            && controller.m_Grounded)
        {
            

            //Attack anim with combo trigger
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

        foreach (Collider2D enemy in hitEnemies)
          
        {
           
            if (enemy == enemy.GetComponent<BoxCollider2D>())
            {
                enemy.GetComponent<Enemy_Instant>().TakeDamage(attackDamage);
              
            }
  
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
       
    }
}


