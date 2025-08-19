using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//半山腰太挤，你总得去山顶看看//
public class MonsterMoveState : MonsterState
{
    public MonsterMoveState(Enity enity, StateMachine stateMachine, Core core, string animName, MonsterEnity monsterEnity) : base(enity, stateMachine, core, animName, monsterEnity)
    {
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
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
