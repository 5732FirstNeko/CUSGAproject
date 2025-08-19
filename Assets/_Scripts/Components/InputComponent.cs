using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;

//半山腰太挤，你总得去山顶看看//
public class InputComponent : CoreComponent
{
    [SerializeField] private float attackBufferTime;

    private PlayerActions playerActions;

    private Vector2 movementAction;

    private bool jumpAction;

    public float xInput { get; private set; }
    public float yInput { get; private set; }

    public bool jumpInput { get; private set; }
    public bool dashInput { get; private set; }

    public AttackInputTypes attackInput { get; private set; } = new AttackInputTypes();

    protected override void Awake()
    {
        base.Awake();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        ActionControll();
    }

    private void ActionControll()
    {
        if (movementAction.x > 0.5)
        {
            xInput = 1;
        }
        else if (movementAction.x > 0 && movementAction.x < 0.5f)
        {
            xInput = 0;
        }
        else if (movementAction.x < 0 && movementAction.x > -0.5f)
        {
            xInput = 0;
        }
        else if (movementAction.x < -0.5f)
        {
            xInput = -1;
        }
        else if (movementAction.x == 0)
        {
            xInput = 0;
        }

        jumpInput = jumpAction;

        if (!attackInput.X_input && !attackInput.Y_input)
        {
            attackInput.isAttack = false;
        }
        else
        {
            attackInput.isAttack = true;
        }
    }

    private void Start()
    {
        playerActions = new PlayerActions();
        playerActions.Enable();
        playerActions.PlayerControl.Move.performed += OnMovePerformed;
        playerActions.PlayerControl.Move.canceled += OnMoveCanceled;
        playerActions.PlayerControl.Jump.performed += OnJumpPreformed;
        playerActions.PlayerControl.Attack.performed += OnX_AttackPerform;
        playerActions.PlayerControl.Y_Attack.performed += OnY_AttackPerform;
    }

    private void OnDestroy()
    {
        playerActions.PlayerControl.Move.performed -= OnMovePerformed;
        playerActions.PlayerControl.Move.canceled -= OnMoveCanceled;
        playerActions.PlayerControl.Jump.performed -= OnJumpPreformed;
        playerActions.PlayerControl.Attack.performed -= OnX_AttackPerform;
        playerActions.PlayerControl.Y_Attack.performed -= OnY_AttackPerform;
    }

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        movementAction = context.ReadValue<Vector2>();
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        movementAction.x = 0;
    }

    private void OnJumpPreformed(InputAction.CallbackContext context)
    {
        jumpAction = true;
        StartCoroutine(ResetJumpAfterFrame());
    }

    IEnumerator ResetJumpAfterFrame()
    {
        yield return null;  // 等待一帧
        jumpAction = false;
    }

    public void SetJumpFalse()
    {
        jumpInput = false;
    }

    public void StopAllAttackBuutonTimer()
    {
        GameManager.instance.StopTimer("AttackBuffer");
    }

    private void OnX_AttackPerform(InputAction.CallbackContext context)
    {
        attackInput.SetAllButtonFalse();
        attackInput.X_input = true;
        GameManager.instance.StartTimer("AttackBuffer",attackBufferTime,()=>attackInput.X_input = false);
    }

    private void OnY_AttackPerform(InputAction.CallbackContext context)
    {
        attackInput.SetAllButtonFalse();
        attackInput.Y_input = true;
        GameManager.instance.StartTimer("AttackBuffer", attackBufferTime, () => attackInput.Y_input = false);
    }
}
