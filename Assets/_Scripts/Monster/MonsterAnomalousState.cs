using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//半山腰太挤，你总得去山顶看看//
public class MonsterAnomalousState : MonsterState
{
    public MonsterAnomalousState(Enity enity, StateMachine stateMachine, Core core, string animName, MonsterEnity monsterEnity) : base(enity, stateMachine, core, animName, monsterEnity)
    {
    }
}

public class MonsterAnomalousState<T> : MonsterAnomalousState where T : AnomalousStateData
{
    protected T anomalousData;

    public MonsterAnomalousState(Enity enity, StateMachine stateMachine, Core core, string animName, MonsterEnity monsterEnity) : base(enity, stateMachine, core, animName, monsterEnity)
    {
        
    }

    public virtual void SetAnomalousStateData(T data)
    {
        anomalousData = data;
    }
}

