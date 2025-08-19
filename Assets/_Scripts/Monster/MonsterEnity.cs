using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//半山腰太挤，你总得去山顶看看//
public class MonsterEnity : Enity
{
    public MonsterIdleState idleState { get; private set; }
    public MonsterKnockBackState knockBackState { get; private set; }
    public MonsterCompulsionState compulsionState { get; private set; }

    protected override void Start()
    {
        base.Start();

        idleState = new MonsterIdleState(this,stateMachine,core,"Idle",this);
        knockBackState = new MonsterKnockBackState(this,stateMachine,core,"KnockBack",this);
        compulsionState = new MonsterCompulsionState(this, stateMachine, core, "Idle", this);

        stateMachine.Initialize(idleState);
    }

    public override void KnockBackStateEnter(KnockBackStateData anomalousData)
    {
        knockBackState.SetAnomalousStateData(anomalousData);
        stateMachine.ChangState(knockBackState);
    }

    public override void CompulsionStateEnter(CompulsionStateData anomalousData)
    {
        compulsionState.SetAnomalousStateData(anomalousData);
        stateMachine.ChangState(compulsionState);
    }
}
