using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//半山腰太挤，你总得去山顶看看//
public class PlayerAbilityState : PlayerState
{
    protected bool isAbilityFinished;

    public PlayerAbilityState(Enity enity, StateMachine stateMachine, Core core, string animName, PlayerEnity playerEnity) : base(enity, stateMachine, core, animName, playerEnity)
    {
    }

    public override void Enter()
    {
        base.Enter();

        isAbilityFinished = false;
    }

    public override void Exit()
    {
        base.Exit();

        isAbilityFinished = true;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isAbilityFinished)
        {
            if (collisionComponent.isGrounded)
            {
                stateMachine.ChangState(playerEnity.idleState);
            }
            else if (!collisionComponent.isGrounded)
            {
                stateMachine.ChangState(playerEnity.inAirState);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    protected virtual void AbilityFinished()
    {
        isAbilityFinished = true;
    }
}
