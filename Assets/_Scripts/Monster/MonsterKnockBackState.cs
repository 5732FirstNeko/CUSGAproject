using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//半山腰太挤，你总得去山顶看看//
public class MonsterKnockBackState : MonsterAnomalousState<KnockBackStateData>
{
    public MonsterKnockBackState(Enity enity, StateMachine stateMachine, Core core, string animName, MonsterEnity monsterEnity) : base(enity, stateMachine, core, animName, monsterEnity)
    {
    }

    public override void Enter()
    {
        base.Enter();
        moveComponent.SetVelocity(anomalousData.knockBackVelocity);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (moveComponent.CurrentVelocity.magnitude < 0.1f || Time.time > startTime + anomalousData.knockBackTime)
        {
            stateMachine.ChangState(monsterEnity.idleState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void SetAnomalousStateData(KnockBackStateData data)
    {
        base.SetAnomalousStateData(data);
    }
}
