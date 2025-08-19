using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//半山腰太挤，你总得去山顶看看//
public class PlayerLandState : PlayerGroundedState
{
    public PlayerLandState(Enity enity, StateMachine stateMachine, Core core, string animName, PlayerEnity playerEnity) : base(enity, stateMachine, core, animName, playerEnity)
    {
    }

    public override void Enter()
    {
        base.Enter();

        moveComponent.SetVelocityZero();
        if (inputComponent.xInput != 0)
        {
            moveComponent.SetVelocityX(playerEnity.EnityData.MoveVelocity);
            stateMachine.ChangState(playerEnity.moveState);
        }
        else if (inputComponent.xInput == 0)
        {
            stateMachine.ChangState(playerEnity.idleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
