using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//半山腰太挤，你总得去山顶看看//
public interface IAnomalousStateEnter
{
    public abstract void KnockBackStateEnter(KnockBackStateData anomalousData);

    public abstract void CompulsionStateEnter(CompulsionStateData anomalousData);

    public abstract void LevitateStateEnter(LevitateStateData anomalousData);
}
