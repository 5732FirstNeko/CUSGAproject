using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//半山腰太挤，你总得去山顶看看//
public class PlayerMoveState : PlayerGroundedState
{
    public PlayerMoveState(Enity enity, StateMachine stateMachine, Core core, string animName, PlayerEnity playerEnity) : base(enity, stateMachine, core, animName, playerEnity)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (inputComponent.xInput != moveComponent.FacingDirection && inputComponent.xInput != 0)
        {
            moveComponent.Flip();
        }

        if(inputComponent.xInput == 0)
        {
            stateMachine.ChangState(playerEnity.idleState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (Mathf.Abs(moveComponent.CurrentVelocity.x) < statsComponent.MoveSpeed)
        {
            moveComponent.SetForeceX(statsComponent.StartForce * moveComponent.FacingDirection, false);
        }
        else
        {
            moveComponent.SetVelocityX(statsComponent.MoveSpeed);
        }
    }
}
