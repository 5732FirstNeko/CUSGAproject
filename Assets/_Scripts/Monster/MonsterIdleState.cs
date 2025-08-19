using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//半山腰太挤，你总得去山顶看看//
public class MonsterIdleState : MonsterGrounded
{
    public MonsterIdleState(Enity enity, StateMachine stateMachine, Core core, string animName, MonsterEnity monsterEnity) : base(enity, stateMachine, core, animName, monsterEnity)
    {
    }

    public override void Enter()
    {
        base.Enter();

        moveComponent.SetVelocityZero();
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
