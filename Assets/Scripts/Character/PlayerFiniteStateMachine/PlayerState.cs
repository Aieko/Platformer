﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    protected Player player;
    protected PlayerStateMachine stateMachine;
    protected PlayerData playerData;

    protected float startTime;

    protected bool isAnimationFinished;

    private string animBoolName;

    public PlayerState(Player player, PlayerStateMachine playerStateMachine, PlayerData playerData, string animBoolName)
    {
        this.player = player;
        this.stateMachine = playerStateMachine;
        this.playerData = playerData;
        this.animBoolName = animBoolName;
    }

    public virtual void Enter()
    {
        DoChecks();

        startTime = Time.time;

        player.Anim.SetBool(animBoolName, true);

        Debug.Log(animBoolName);

        isAnimationFinished = false;
    }

    public virtual void Exit()
    {
        player.Anim.SetBool(animBoolName, false);
    }

    public virtual void LogicUpdate()
    {

    }

    public virtual void PhysicsUpdate()
    {
        DoChecks();
    }

    public virtual void DoChecks()
    {

    }

    public virtual void AnimationTrigger()
    {

    }

    public virtual void AnimationFinishTrigger() => isAnimationFinished = true;
 

}