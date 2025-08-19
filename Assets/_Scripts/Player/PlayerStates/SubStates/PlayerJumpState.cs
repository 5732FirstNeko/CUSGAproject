using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//半山腰太挤，你总得去山顶看看//
public class PlayerJumpState : PlayerAbilityState
{
    public PlayerJumpState(Enity enity, StateMachine stateMachine, Core core, string animName, PlayerEnity playerEnity) : base(enity, stateMachine, core, animName, playerEnity)
    {
    }

    public override void Enter()
    {
        base.Enter();

        inputComponent.SetJumpFalse();
        moveComponent.SetForce(Vector2.zero,false);
        moveComponent.SetVelocityY(playerEnity.EnityData.jumpVelocity);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!collisionComponent.isGrounded)
        {
            isAbilityFinished = true;
            stateMachine.ChangState(playerEnity.inAirState);
        }
    }
}
