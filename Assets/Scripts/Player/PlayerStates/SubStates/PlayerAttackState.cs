using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : PlayerAbilityState
{
    private Weapon weapon;

    private int xInput;

    private bool shouldCheckFlip;

    private float velocityToSet;
    private bool setVelocity;

    public PlayerAttackState(Player player, PlayerStateMachine playerStateMachine, PlayerData playerData, string animBoolName) : base(player, playerStateMachine, playerData, animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();

        setVelocity = false;
        if(weapon)
        weapon.EnterWeapon();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        xInput = player.InputHandler.NormInputX;

        if(shouldCheckFlip)
        {
            core.Movement.CheckIfShouldFlip(xInput);
        }

        if(setVelocity)
        {
            core.Movement.SetVelocityX(velocityToSet * core.Movement.FacingDirection);
        }
    }

    public override void Exit()
    {
        base.Exit();
        if(weapon)
        weapon.ExitWeapon();
    }

    public void SetWeapon(Weapon weapon)
    {
        if(weapon)
        {
            this.weapon = weapon;

            weapon.InitializeWeapon(this);
        }
        
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        isAbilityDone = true;
    }

    public void SetPlayerVelocity(float velocity)
    {
        core.Movement.SetVelocityX(velocity * core.Movement.FacingDirection);

        velocityToSet = velocity;

        setVelocity = true;
    }

    public void SetFlipCheck(bool value)
    {
        shouldCheckFlip = value;
    }
}
