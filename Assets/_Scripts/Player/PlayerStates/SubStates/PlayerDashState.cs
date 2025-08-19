using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//半山腰太挤，你总得去山顶看看//
public class PlayerDashState : PlayerAbilityState
{
    public PlayerDashState(Enity enity, StateMachine stateMachine, Core core, string animName, PlayerEnity playerEnity) : base(enity, stateMachine, core, animName, playerEnity)
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

        if (Time.time >= startTime + statsComponent.DashTime)
        {
            stateMachine.ChangState(playerEnity.idleState);
        }
        else if (collisionComponent.isTouchWall)
        {
            stateMachine.ChangState(playerEnity.idleState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        moveComponent.SetVelocityX(statsComponent.DashSpeed);
    }
}
