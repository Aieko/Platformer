using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "NewPlayerData", menuName = "Data/Player Data/Base Date")]

public class PlayerData : ScriptableObject
{
    //TODO need to clear some properties that doubles in collisionSenses and Movement

    [Header("Move State")]
    public float movementVelocity = 10f;
    public float standColliderHeight = 1.5f;
    
    [Header("Jump State")]
    public float jumpVelocity = 15f;
    public int amountOfJumps = 1;

    [Header("In Air State")]
    public float coyoteTime = 0.15f;
    public float jumpHeightMultiplier = 0.5f;

    [Header("Wall Slide State")]
    public float wallSlideVelocity = 2f;

    [Header("Wall Jump State")]
    public float wallJumpVelocity = 20f;
    public float wallJumpTime = 0.4f;
    public Vector2 wallJumpAngle = new Vector2(1, 2);


    [Header("Wall Climb State")]
    public float wallClimbVelocity = 2f;

    [Header("Ledge Climb State")]
    public Vector2 startOffset;
    public Vector2 stopOffset;

    [Header("Dash State")]
    public float dashCoolDown = 0.5f;
    public float maxHoldTime = 1f;
    public float holdTimeScale = 0.25f;
    public float dashTime = 0.2f;
    public float dashVelocity = 30f;
    public float drag = 10f;
    public float dashEndYMultiplier = 0.2f;
    public float distBetweenAfterImages = 0.5f;

    [Header("Crouch State")]
    public float crouchMovementVelocity = 5f;
    public float crouchColliderHeight = 0.8f;

}
