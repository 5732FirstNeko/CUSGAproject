using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//半山腰太挤，你总得去山顶看看//
public class PlayerAttackEndState : PlayerState
{
    protected PlayerWeapon weapon;

    public PlayerAttackEndState(Enity enity, StateMachine stateMachine, Core core, string animName, PlayerEnity playerEnity) : base(enity, stateMachine, core, animName, playerEnity)
    {
    }

    public override void Enter()
    {
        base.Enter();

        weapon = playerEnity.weapon;

        if (!inputComponent.attackInput.isAttack)
        {
            if (collisionComponent.isGrounded)
            {
                inputComponent.StopAllAttackBuutonTimer();
                inputComponent.attackInput.SetAllButtonFalse();
                stateMachine.ChangState(playerEnity.idleState);
            }
            else
            {
                inputComponent.StopAllAttackBuutonTimer();
                inputComponent.attackInput.SetAllButtonFalse();
                stateMachine.ChangState(playerEnity.inAirState);
            }
        }
        else if(inputComponent.attackInput.isAttack)
        {
            stateMachine.ChangState(playerEnity.attackState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (inputComponent.attackInput.isAttack)
        {
            stateMachine.ChangState(playerEnity.attackState);
        }
        else if (collisionComponent.isGrounded)
        {
            inputComponent.StopAllAttackBuutonTimer();
            inputComponent.attackInput.SetAllButtonFalse();
            stateMachine.ChangState(playerEnity.idleState);
        }
        else if(!collisionComponent.isGrounded)
        {
            inputComponent.StopAllAttackBuutonTimer();
            inputComponent.attackInput.SetAllButtonFalse();
            stateMachine.ChangState(playerEnity.inAirState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
