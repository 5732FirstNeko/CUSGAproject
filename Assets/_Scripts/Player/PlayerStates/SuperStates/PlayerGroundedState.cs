using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//半山腰太挤，你总得去山顶看看//
public class PlayerGroundedState : PlayerState
{
    public PlayerGroundedState(Enity enity, StateMachine stateMachine, Core core, string animName, PlayerEnity playerEnity) : base(enity, stateMachine, core, animName, playerEnity)
    {
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!collisionComponent.isGrounded)
        {
            stateMachine.ChangState(playerEnity.inAirState);
        }
        else if (inputComponent.jumpInput)
        {
            stateMachine.ChangState(playerEnity.jumpState);
        }
        else if (inputComponent.dashInput)
        {

        }
        else if (inputComponent.attackInput.isAttack)
        {
            stateMachine.ChangState(playerEnity.attackState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
