﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "NewPlayerData", menuName = "Data/Player Data/Base Date")]

public class PlayerData : ScriptableObject
{
    [Header("Move State")]
    public float movementVelocity = 10f;

    [Header("Jump State")]
    public float jumpVelocity = 15f;
    public int amountOfJumps = 1;

    [Header("In Air State")]
    public float coyoteTime = 0.15f;
    public float jumpHeightMultiplier = 0.5f;

    [Header("Wall Slide State")]
    public float wallSlideVelocity = 2f;

    [Header("Wall Climb State")]
    public float wallClimbVelocity = 2f;

    [Header("Check Veriables")]
    public float GroundCheckRadius = 0.3f;
    public LayerMask whatIsGround;
    public float wallCheckDistance = 1f;
    

}
