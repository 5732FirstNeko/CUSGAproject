using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//半山腰太挤，你总得去山顶看看//
public class PlayerAnomalousState : State
{
    protected PlayerEnity playerEnity;
    public PlayerAnomalousState(Enity enity, StateMachine stateMachine, Core core, string animName,PlayerEnity playerEnity) : base(enity, stateMachine, core, animName)
    {
        this.playerEnity = playerEnity;
    }
}

public class PlayerAnomalousState<T> : PlayerAnomalousState where T : AnomalousStateData
{
    protected T anomalousData;

    public PlayerAnomalousState(Enity enity, StateMachine stateMachine, Core core, string animName, PlayerEnity playerEnity) : base(enity, stateMachine, core, animName, playerEnity)
    {
    }

    public virtual void SetAnomalousStateData(T data)
    {
        anomalousData = data;
    }
}
