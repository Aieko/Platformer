using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeCombat : MonoBehaviour
{
    [SerializeField]
    private float stunDamageAmount = 1f;
    [SerializeField]
    private bool combatEnabled;
    [SerializeField]
    private Transform attack1HitBoxPos;
    [SerializeField]
    private LayerMask whatIsDamageable;
    [SerializeField]
    private float inputTimer, attack1Radius, attack1Damage;

    private Animator anim;
    private bool gotInput, isAttacking, isFirstAttack;

    private AttackDetails attackDetails;
    private float lastInputTime = Mathf.NegativeInfinity;

    private CharacterController2D PC;

    private CharacterStats PS;

    private void Update()
    {
        CheckCombatInput();
        CheckAttacks();
    }

    private void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("canAttack", combatEnabled);

        PC = GetComponent<CharacterController2D>();
        PS = GetComponent<CharacterStats>(); //player stats
    }

    private void CheckCombatInput()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if(combatEnabled)
            {
                gotInput = true;
                lastInputTime = Time.time;
            }
        }

    }
  
    private void CheckAttacks()
    {
        if(gotInput)
        {
            //attack
            if(!isAttacking)
            {
                gotInput = false;
                isAttacking = true;
                isFirstAttack = !isFirstAttack;
                anim.SetBool("attack1", true);
                anim.SetBool("firstAttack", isFirstAttack);
                anim.SetBool("isAttacking", isAttacking);

            }
        }

        if(Time.time >=lastInputTime + inputTimer)
        {
            gotInput = false;
        }
    }

    private void CheckAttackHitBox()
    {
        Collider2D[] detectObjects = Physics2D.OverlapCircleAll(attack1HitBoxPos.position, attack1Radius, whatIsDamageable);

        attackDetails.damageAmount = attack1Damage;
        attackDetails.position = transform.position;
        attackDetails.stunDamageAmount = stunDamageAmount;

        Debug.Log(detectObjects.Length);
        foreach (Collider2D collider in detectObjects)
        {   
            if(collider.transform.gameObject.layer == 9)
            collider.transform.parent.SendMessage("Damage", attackDetails);

            //hit particle
        }
    }

    private void Damage(AttackDetails attackDetails)
    {
        if (!PC.GetDashStatus())
        {
            int direction;

            PS.DecreaseHealth(attackDetails.damageAmount);

            if (attackDetails.position.x < transform.position.x)
            {
                direction = 1;
            }
            else
            {
                direction = -1;
            }

            PC.Knockback(direction);
        }
    }

    private void FinishAttack1()
    {
        isAttacking = false;

        anim.SetBool("isAttacking", isAttacking);
        anim.SetBool("attack1", false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attack1HitBoxPos.position, attack1Radius);
    }
}


