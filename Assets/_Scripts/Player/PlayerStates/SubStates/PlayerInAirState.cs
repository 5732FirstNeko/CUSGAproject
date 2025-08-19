using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

//半山腰太挤，你总得去山顶看看//
public class PlayerInAirState : PlayerState
{
    public PlayerInAirState(Enity enity, StateMachine stateMachine, Core core, string animName, PlayerEnity playerEnity) : base(enity, stateMachine, core, animName, playerEnity)
    {
    }

    public override void Enter()
    {
        base.Enter();

        inputComponent.attackInput.isInAir = true;
    }

    public override void Exit()
    {
        base.Exit();

        inputComponent.attackInput.isInAir = false;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (collisionComponent.isGrounded)
        {
            stateMachine.ChangState(playerEnity.landState);
        }
        else if (inputComponent.attackInput.isAttack)
        {
            stateMachine.ChangState(playerEnity.attackState);
        }
        else if (inputComponent.xInput != 0)
        {
            if (inputComponent.xInput != moveComponent.FacingDirection)
            {
                moveComponent.Flip();
            }
            moveComponent.SetVelocityX(statsComponent.AirMoveSpeed);
        }

        if (moveComponent.CurrentVelocity.y >= 0)
        {
            enity.animator.SetFloat("AirState",-1);
        }
        else
        {
            enity.animator.SetFloat("AirState", 1);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
