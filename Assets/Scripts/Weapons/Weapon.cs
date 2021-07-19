using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] protected SO_WeaponData weaponData;

    protected Animator baseAnimator;
    protected Animator weaponAnimator;

    protected PlayerAttackState state;

    protected int attackCounter;

    protected virtual void Awake()
    {
        baseAnimator = transform.Find("Base").GetComponent<Animator>();
        weaponAnimator = transform.Find("Weapon").GetComponent<Animator>();

        gameObject.SetActive(false);
    }

    public virtual void EnterWeapon()
    {
        gameObject.SetActive(true);

        if (attackCounter >= weaponData.amountOfAttacks)
        {
            attackCounter = 0;
        }

        baseAnimator.SetBool("Attack", true);
        weaponAnimator.SetBool("Attack", true);

        baseAnimator.SetInteger("AttackCounter", attackCounter);
        weaponAnimator.SetInteger("AttackCounter", attackCounter);
    }

    public virtual void AnimationStartMovementTrigger()
    {
        state.SetPlayerVelocity(weaponData.movementSpeed[attackCounter]);
    }

    public virtual void AnimationStopMovementTrigger()
    {
        state.SetPlayerVelocity(0f);
    }

    public virtual void AnimationTurnOffFlip()
    {
        state.SetFlipCheck(false);
    }

    public virtual void AnimationTurnOnFlip()
    {
        state.SetFlipCheck(true);
    }

    public virtual void ExitWeapon()
    {
        baseAnimator.SetBool("Attack", false);
        weaponAnimator.SetBool("Attack", false);

        attackCounter++;

        gameObject.SetActive(false);
    }

    #region Weapon Triggers

    public virtual void AnimationFinishTrigger()
    {
        state.AnimationFinishTrigger();
    }

    #endregion

    public void InitializeWeapon(PlayerAttackState state)
    {
        this.state = state;
    }

    public virtual void AnimationActionTrigger()
    {

    }
}
