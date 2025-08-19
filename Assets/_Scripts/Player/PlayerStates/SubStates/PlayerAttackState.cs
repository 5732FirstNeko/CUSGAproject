using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//半山腰太挤，你总得去山顶看看//
public class PlayerAttackState : PlayerAbilityState
{
    protected PlayerWeapon weapon;

    public bool canExitAttack_Move = false;
    public bool canExitAttack_Attack = false;

    public PlayerAttackState(Enity enity, StateMachine stateMachine, Core core, string animName, PlayerEnity playerEnity) : base(enity, stateMachine, core, animName, playerEnity)
    {
    }

    public override void Enter()
    {
        base.Enter();
        moveComponent.SetVelocityZero();
        canExitAttack_Move = false;
        canExitAttack_Attack = false;

        weapon = playerEnity.weapon;
        weapon.AttackEndEvent += AbilityFinished;
        weapon.AttackCanExit_Move += SetCanExitAttack_Move;
        weapon.AttackCanExit_Attack += SetCanExitAttack_Attack;
        weapon.EnterAttack(inputComponent.attackInput);

        inputComponent.StopAllAttackBuutonTimer();
        inputComponent.attackInput.SetAllButtonFalse();
    }

    public override void Exit()
    {
        base.Exit();

        weapon.ExitAttack();
        weapon.AttackCanExit_Attack -= SetCanExitAttack_Attack;
        weapon.AttackCanExit_Move -= SetCanExitAttack_Move;
        weapon.AttackEndEvent -= AbilityFinished;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (canExitAttack_Attack && !canExitAttack_Move)
        {
            if (inputComponent.xInput != moveComponent.FacingDirection && inputComponent.xInput != 0)
            {
                moveComponent.Flip();
            }

            if (inputComponent.attackInput.isAttack)
            {
                stateMachine.ChangState(playerEnity.attackState);
            }
        }

        if (canExitAttack_Move)
        {
            if (inputComponent.attackInput.isAttack)
            {
                stateMachine.ChangState(playerEnity.attackState);
            }
            else if (inputComponent.xInput != 0)
            {
                inputComponent.StopAllAttackBuutonTimer();
                inputComponent.attackInput.SetAllButtonFalse();
                stateMachine.ChangState(playerEnity.attackEndState);
            }
            else if (inputComponent.jumpInput)
            {
                inputComponent.StopAllAttackBuutonTimer();
                inputComponent.attackInput.SetAllButtonFalse();
                stateMachine.ChangState(playerEnity.jumpState);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    private void SetCanExitAttack_Move()
    {
        canExitAttack_Move = true;
    }

    private void SetCanExitAttack_Attack()
    {
        canExitAttack_Attack = true;
    }
}
