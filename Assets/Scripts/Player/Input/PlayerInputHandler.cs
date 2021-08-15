using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerInputHandler : MonoBehaviour
{
    private PlayerInput playerInput;

    private Camera cam;

    private GameObject gameMenu;

    public Vector2 RawDashDirectionInput { get; private set; }

    public Vector2 RawMovementInput { get; private set; }

    public Vector2Int DashDirectionInput { get; private set; }

    public int NormInputX { get; private set; }

    public int NormInputY { get; private set; }

    public bool JumpInput { get; private set; }

    public bool JumpInputStop { get; private set; }

    public bool GrabInput { get; private set; }

    public bool DashInput { get; private set; }

    public bool DashInputStop { get; private set; }

    public bool[] AttackInputs { get; private set; }

    public bool isGamePaused;

    [SerializeField]
    private float InputHoldTime = 0.15f;

    private float jumpInputStartTime;

    private float dashInputStartTime;

    

    #region Callback functions

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        cam = Camera.main;
    }

    private void Start()
    {
        gameMenu = GameObject.FindGameObjectWithTag("GameMenu");

        int count = Enum.GetValues(typeof(CombatInputs)).Length;

        AttackInputs = new bool[count];
    }

    private void Update()
    {
        CheckJumpInputHoldTime();
        CheckDashInputHoldTime();
    }

    #endregion

    #region OnInput

    public void OnEscapeInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            gameMenu.GetComponent<Canvas>().enabled = true;
            isGamePaused = true;
            playerInput.DeactivateInput();
            Time.timeScale = 0f;
        }

    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        RawMovementInput = context.ReadValue<Vector2>();

        NormInputX = Mathf.RoundToInt(RawMovementInput.x);

        NormInputY = Mathf.RoundToInt(RawMovementInput.y);

    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
       if(context.started)
       {
           JumpInput = true;
           jumpInputStartTime = Time.time;
           JumpInputStop = false; 
       }

       if(context.canceled)
       {
           JumpInputStop = true;
       }
    }

    public void OnDashInput(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            DashInput = true;
            DashInputStop = false;
            dashInputStartTime = Time.time;

        }
        else if(context.canceled)
        {
            DashInputStop = true;
        }
    }

    public void OnDashDirectionInput(InputAction.CallbackContext context)
    {
        RawDashDirectionInput = context.ReadValue<Vector2>();

        if(playerInput.currentControlScheme == "Keyboard" && cam)
        {
            RawDashDirectionInput = cam.ScreenToWorldPoint((Vector3)RawDashDirectionInput) - transform.position;
        }

        DashDirectionInput = Vector2Int.RoundToInt(RawDashDirectionInput.normalized);

    }

    public void OnGrabInput(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            GrabInput = true;
        }
        else if(context.canceled)
        {
            GrabInput = false;
        }
    }

    public void OnPrimaryAttackInput(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            AttackInputs[(int)CombatInputs.primary] = true;
        }

        if(context.canceled)
        {
            AttackInputs[(int)CombatInputs.primary] = false;
        }
    }

    public void OnSecondaryAttackInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            AttackInputs[(int)CombatInputs.secondary] = true;
        }

        if (context.canceled)
        {
            AttackInputs[(int)CombatInputs.secondary] = false;
        }
    }

    #endregion

    #region Checks

    private void CheckJumpInputHoldTime()
    {
        if(Time.time >=jumpInputStartTime + InputHoldTime)
        {
            JumpInput = false;
        }
    }

    private void CheckDashInputHoldTime()
    {
        if(Time.time >= dashInputStartTime + InputHoldTime)
        {
            DashInput = false;
        }
    }

    #endregion

    #region Use Input

    public void UseJumpInput() => JumpInput = false;

    public void UseDashInput() => DashInput = false;

    #endregion

    #region OtherMethods

    public void ActivateInput(bool value)
    {
        if(value)
        {
            playerInput.ActivateInput();
        }
        else
        {
            playerInput.DeactivateInput();
        }
    }

    #endregion
}

public enum CombatInputs
{
    primary,
    secondary
}
